using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AssortmentManagement.Model;
using AssortmentManagement.UserValues;
using DevExpress.Xpf.PivotGrid;
using DevExpress.XtraPivotGrid.Localization;
using DevExpress.Xpf.Grid;
using SharedComponents.Localization;

namespace AssortmentManagement.Controls
{
    public class PivotGridControlModified : PivotGridControl
    {
        #region Fields

        public delegate void CellActionEventHandler(object sender, CellActionEventArgs e);
        public event CellActionEventHandler CellClickAction;
        public FormTypes FormType { get; set; }
        public CellActionEventArgs CellArgs { get; set; }

        #endregion
        
        public void InitializeControl(List<Column> dimensions, FormTypes formType)
        {
            FilterByVisibleFieldsOnly = true;
            FormType = formType;
            CellArgs = new CellActionEventArgs();
            PivotGridLocalizer.Active = new DXPivotGridLocalizerRU();
            GridControlLocalizer.Active = new DXGridControlLocalizerRU();

            InitializeFields(dimensions);

            if (FormType==FormTypes.Secondary)
            {
                //LocTypeSOnly();
                Fields["DIM_LOC_TYPE"].Area = FieldArea.FilterArea;
                Fields["DIM_LOC_TYPE"].Visible = true;
                Fields["DIM_LOC_TYPE"].AllowDrag = false;
            }

            #region Initialize Event Handlers

            FieldAreaChanging += PivotGridControlModifiedFieldAreaChanging;
            FieldFilterChanged += PivotGridControlModifiedFieldFilterChanged;
            CustomSummary += PivotGridControlModifiedCustomSummary;
            CellDoubleClick += PivotGridControlModifiedCellClick;

            #endregion

            //SetFieldListSize(Size.Empty, new Size(250, 600));
        }

        public List<string> GetVisibleFields()
        {
            var fieldlist=GetFieldsByArea(FieldArea.RowArea);
            fieldlist.AddRange(GetFieldsByArea(FieldArea.FilterArea));
            var list = fieldlist.Select(pivotGridField => pivotGridField.FieldName).ToList();
            return list;
        }

        public void VisibleAddedItems(Dictionary<string,FilterValues> filters)
        {
            foreach (var key in filters.Keys)
            {
                //Fields[key.ToString()].FilterValues.ValuesIncluded = 
                var list = filters[key].Values.ToList();
                var t = Fields[key].FilterValues.ValuesIncluded.ToList();
                t.AddRange(list);
                Fields[key].FilterValues.ValuesIncluded = t.Distinct().ToArray();
            }
        }


        public void FieldExclude(string fieldName, object o)
        {
            if (Fields[fieldName] == null) throw new AssortmentException("Field " + fieldName + " does not exist");
            foreach (var t in Fields[fieldName].FilterValues.ValuesIncluded.Where(t => t.Equals(o)))
            {
                Fields[fieldName].FilterValues.ValuesExcluded = new[] { t };
            }
            Fields[fieldName].AllowFilter = false;
            Fields[fieldName].Area = FieldArea.FilterArea;
            Fields[fieldName].Visible = true;
        }

        public void SetFiltersForWhRestExistsCheckErrorDoc()
        {
            RaiseFieldFilterChanged(Fields.First(t => t.FieldName=="DIM_LOC_TYPE"));
        }

        public void LocTypeSOnly()
        {
            foreach (var t in Fields["DIM_LOC_TYPE"].FilterValues.ValuesIncluded.Where(t => Convert.ToChar(t) == (char)LocTypes.W))
            {
                Fields["DIM_LOC_TYPE"].FilterValues.ValuesExcluded = new[] { t };
            }
            Fields["DIM_LOC_TYPE"].AllowFilter = false;
        }

        public void ItemTypeExpendMaterialOnly()
        {
            foreach (var t in Fields["DIM_ITEM_TYPE"].FilterValues.ValuesIncluded.Where(t => t.ToString() == ItemTypes.Item))
            {
                Fields["DIM_ITEM_TYPE"].FilterValues.ValuesExcluded = new[] { t };
            }
            Fields["DIM_ITEM_TYPE"].AllowFilter = false;
        }

        private void InitializeFields(IEnumerable<Column> dimensions)
        {
            
            foreach (var dimension in dimensions)
            {
                if (dimension.Name == "DIM_ITEMLOC_TRANSITWH" || dimension.Name == "DIM_ITEMLOC_STATUS_OLD" || dimension.Name == "DIM_ITEM_STANDARD_EQUIP") continue;
                
                var field = new PivotGridField { FieldName = dimension.Name, Caption = dimension.Desc, Visible = false };

                if (dimension.Name.Contains("ITEMLOC"))
                {
                    field.CellFormat = "{0}";
                    field.SummaryType = FieldSummaryType.Average;
                }
                else if (dimension.Name.Contains("ITEM"))
                {
                    if (dimension.Name.Equals("DIM_ITEM_DESC"))
                    {
                        field.Width = 350;
                    }
                }
                else if (dimension.Name.Contains("LOC"))
                {
                }
                else if (dimension.Name.Contains("MEASURE"))
                {
                    field.CellFormat = "{0}";
                    switch (FormType)
                    {
                        case FormTypes.Base:
                            {
                                field.SummaryType = FieldSummaryType.Sum;
                                break;
                            }
                        case FormTypes.Secondary:
                            {
                                field.SummaryType = FieldSummaryType.Custom;
                                field.CellTemplate = (DataTemplate)FindResource("CellTemplate");
                                break;
                            }
                    }
                }
                Fields.Add(field);
            }}

        public void SetFilters(string fieldName, List<string> values)
        {
            var field = Fields[fieldName];
            field.FilterValues.ValuesIncluded = values.ToArray();
        }

        private void CopyArea(PivotGridControl control, FieldArea area)
        {
            foreach (var field in control.GetFieldsByArea(area))
            {
                PivotGridField f = field.FieldName.Equals("MEASURE_STATUS") ? Fields["MEASURE_STATUS_NEW"] : Fields[field.FieldName];
                f.Visible = true;
                f.Area = area;

                if (field.FieldName.Contains("LOC"))
                {
                    f.FilterValues.ValuesIncluded = field.GetVisibleValues().ToArray();
                }
                else if (field.FieldName.Contains("MEASURE") == false)
                {
                    if (field.FilterValues.HasFilter)
                    {
                        f.FilterValues.ValuesIncluded = field.FilterValues.ValuesIncluded;
                    }
                }
            }
        }
        private string GetConditionSpecial(FieldArea area)
        {
            string condition = "";

            var fields = GetFieldsByArea(area);
            foreach (var field in fields)
            {
                if (field.FilterValues.HasFilter == false)
                //if (field.FilterValues.ValuesExcluded.Length == 0)
                {
                    continue;
                }
                if (field.FilterValues.ValuesExcluded.Length == 0)
                {
                    continue;
                }
                if (field.FilterValues.ValuesIncluded.Length == 0)
                {
                    condition = " and 1=2 ";
                    continue;
                }
                if (field.FilterValues.ValuesIncluded.Length < field.FilterValues.ValuesExcluded.Length)
                {
                    condition = condition + " and " + field.FieldName + " in (";
                    for (int i = 0; i < field.FilterValues.ValuesIncluded.Length; i++)
                    {
                        string value;
                        if (field.DataType.Name.Equals("String"))
                        {
                            value = "\'" + field.FilterValues.ValuesIncluded[i] + "\'";
                        }
                        else
                        {
                            value = field.FilterValues.ValuesIncluded[i].ToString();
                        }
                        if (i == field.FilterValues.ValuesIncluded.Length - 1)
                        {
                            condition = condition + value + ")";
                        }
                        else
                        {
                            condition = condition + value + ",";
                        }
                    }
                }
                else
                {
                    condition = condition + " and " + field.FieldName + " not in (";
                    for (int i = 0; i < field.FilterValues.ValuesExcluded.Length; i++)
                    {
                        string value;
                        if (field.DataType.Name.Equals("String"))
                        {
                            value = "\'" + field.FilterValues.ValuesExcluded[i] + "\'";
                        }
                        else
                        {
                            value = field.FilterValues.ValuesExcluded[i].ToString();
                        }
                        if (i == field.FilterValues.ValuesExcluded.Length - 1)
                        {
                            condition = condition + value + ")";
                        }
                        else
                        {
                            condition = condition + value + ",";
                        }
                    }
                }
            }
            return condition;
        }

        #region Event Handlers
        
        private void PivotGridControlModifiedCellClick(object sender, PivotCellEventArgs e)
        {
            if (e.Value == null) return;
            if (e.ColumnField == null || e.RowField == null || e.DataField != Fields["MEASURE_STATUS_NEW"] || FormType == FormTypes.Base || e.Button != MouseButton.Left) return;

            #region Initialize Condition

            var fc = e.GetColumnFields();
            var fr = e.GetRowFields();
            
            var fieldValues = fc.Select(pivotGridField => new FieldValue { Field = pivotGridField.FieldName, Value = e.GetFieldValue(pivotGridField), ShowBlanks = pivotGridField.FilterValues.ShowBlanks}).ToList();
            fieldValues.AddRange(fr.Select(pivotGridField => new FieldValue { Field = pivotGridField.FieldName, Value = e.GetFieldValue(pivotGridField), ShowBlanks = pivotGridField.FilterValues.ShowBlanks }));

            CellArgs.ConditionValues = fieldValues;

            #endregion

            CellArgs.Filters = GetCurrentFiltersState();

            #region Context Menu
            var menu = new ContextMenu();

            for (var i = FormActions.Add; i < FormActions.Restore + 1; i++)
            {
                MenuItem menuItem;
                switch (i)
                {
                    case FormActions.Delete:
                            menuItem = new MenuItem { Tag = i, Header = i.Description(), IsEnabled = (e.DisplayText).Split('&')[3].Equals("0") ? false : true };
                            menuItem.Click += ContextMenuItemClick;
                            menu.Items.Add(menuItem);
                        break;
                    case FormActions.Add:
                        menuItem = new MenuItem { Tag = i, Header = i.Description(), IsEnabled = (e.DisplayText).Split('&')[3].Equals("2") ? false : true };
                            menuItem.Click += ContextMenuItemClick;
                            menu.Items.Add(menuItem);
                        break;
                    default:
                        {
                            menuItem = new MenuItem { Tag = i, Header = i.Description() };
                            menuItem.Click += ContextMenuItemClick;
                            menu.Items.Add(menuItem);
                        }
                        break;
                }
            }
            menu.IsOpen = true;

            #endregion
        }

        public List<FilterValues> GetCurrentFiltersState()
        {
            return (from field in Fields where field.Visible && field.FilterValues.HasFilter select new FilterValues { Field = field.FieldName, Values = field.FilterValues.ValuesIncluded.ToList(), ShowBlanks = field.FilterValues.ShowBlanks }).ToList();
        }

        private void ContextMenuItemClick(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null) CellArgs.ActionType = (FormActions)menuItem.Tag;
            CellClickAction(this, CellArgs);
        }

        private void PivotGridControlModifiedFieldFilterChanged(object sender, PivotFieldEventArgs e)
        {
            if (e.Field.FilterValues.HasFilter == false)
            //if (e.Field.FilterValues.ValuesExcluded.Count() == 0)
            {
                //e.Field.AllowDrag = true;
                e.Field.HeaderTemplate = FieldHeaderTemplate;
                e.Field.HeaderListTemplate = FieldHeaderListTemplate;
            }
            else
            {
                //e.Field.AllowDrag = false;
                //e.Field.AllowDrop = false;
                e.Field.HeaderTemplate = (DataTemplate)FindResource("ItemTemplate");
                e.Field.HeaderListTemplate = (DataTemplate)FindResource("ItemTemplate");
            }

            ItemTypeExpendMaterialOnly();
        }
        private static void PivotGridControlModifiedFieldAreaChanging(object sender, PivotFieldAreaChangingEventArgs e)
        {
            //if (e.Field.FilterValues.ValuesExcluded.Count() != 0)
            //{
            //    e.Allow = e.NewArea == FieldArea.FilterArea;
            //    //e.Field.AllowDrop = true;
            //}
            if (e.Field.FieldName.Contains("ITEMLOC"))
            {
                e.Allow = true;
            }
            else if (e.Field.FieldName.Contains("ITEM"))
            {
                if (e.NewArea == FieldArea.DataArea || e.NewArea == FieldArea.ColumnArea)
                {
                    e.Allow = false;
                }
            }
            else if (e.Field.FieldName.Contains("LOC"))
            {
                if (e.NewArea == FieldArea.DataArea || e.NewArea == FieldArea.RowArea)
                {
                    e.Allow = false;
                }
            }
            else if (e.Field.FieldName.Contains("MEASURE"))
            {
                if (e.NewArea == FieldArea.ColumnArea || e.NewArea == FieldArea.RowArea)
                {
                    e.Allow = false;
                }
            }
        }
        private static void PivotGridControlModifiedCustomSummary(object sender, PivotCustomSummaryEventArgs e)
        {
            var list = e.CreateDrillDownDataSource();
            bool[] action = { false, false, false };
            for (int index = 0; index < list.RowCount; index++)
            {
                PivotDrillDownDataRow row = list[index];

                //  0 : no action
                //  1 : add action
                // -1 : remove action
                //  2 : modify action

                int value = Convert.ToInt32(row["ACTION"]);
                int status = Convert.ToInt32(row["MEASURE_STATUS"]);

                if (value == 1 || value == 4 && status == 0) action[0] = true;
                if (value == -1) action[1] = true;
                if (value == 2 || value == 4 && status == 1) action[2] = true;
            }

            string resultModify = (action[2] ? "1" : "0");
            string resultAction = (action[0] ? "1" : "0") + (action[1] ? "1" : "0");

            string resultSummary = "0";
            if (e.SummaryValue.Max != e.SummaryValue.Min)
            {
                resultSummary = "1";
            }
            else if (Convert.ToInt32(e.SummaryValue.Max) == 1)
            {
                resultSummary = "2";
            }
            else if (Convert.ToInt32(e.SummaryValue.Min) == 0)
            {
                resultSummary = "0";
            }

            e.CustomValue = resultAction + "&" + resultModify + "&" + Convert.ToInt32(e.SummaryValue.Summary) + "&" + resultSummary;
        }
        
        #endregion

        public string GetCondition(List<FieldArea> areas)
        {
            string result = "";
            foreach (var area in areas)
            {
                result += GetConditionSpecial(area);
            }
            //return areas.Select(GetConditionSpecial).Where(result => result.Length != 0).Aggregate("", (current, result) => current + result);
            return result;
        }
        public void CopyLayout(PivotGridControl control)
        {
            CopyArea(control, FieldArea.ColumnArea);
            CopyArea(control, FieldArea.RowArea);
            CopyArea(control, FieldArea.DataArea);
            CopyArea(control, FieldArea.FilterArea);
            Fields["DIM_LOC_TYPE"].AllowDrag = false;
            Fields["DIM_LOC_TYPE"].AllowFilter = false;
        }
        public void ClearLayout()
        {
            foreach (var field in Fields)
            {
                field.Visible = false;
            }
        }
/*
        private void FieldSetValues(KeyValuePair<int, LayoutField> field)
        {
            switch (field.Value.Type)
            {
                case FilterTypes.Included:
                    Fields[field.Value.Name].FilterValues.ValuesIncluded = field.Value.Values;
                    break;
                case FilterTypes.Excluded:
                    Fields[field.Value.Name].FilterValues.ValuesExcluded = field.Value.Values;
                    break;
            }
        }
*/ 
        private void FieldSetValues(LayoutField field)
        {
            switch (field.Type)
            {
                case FilterTypes.Included:
                    Fields[field.Name].FilterValues.ValuesIncluded = field.Values;
                    break;
                case FilterTypes.Excluded:
                    Fields[field.Name].FilterValues.ValuesExcluded = field.Values;
                    break;
            }
        }
        public void SetFilters(List<FieldValue> filterValues)
        {
            foreach (var filterValue in filterValues)
            {
                // extend each filter
                var listValues = Fields[filterValue.Field].FilterValues.ValuesIncluded.ToList();
                if (listValues.Contains(filterValue.Value) == false)
                {
                    listValues.Add(filterValue.Value);
                    Fields[filterValue.Field].FilterValues.ValuesIncluded = listValues.ToArray();
                }
            }
        }

        public void SetLayout(Layout layout)
        {
            if (layout == null) return;

            ClearLayout();

            for (int index = 0; index < layout.ColumnArea.Count; index++)
            {
                LayoutField field;
                if (layout.ColumnArea.TryGetValue(index, out field) == false) continue;
                Fields[field.Name].Area = FieldArea.ColumnArea;
                Fields[field.Name].Visible = true;
                Fields[field.Name].SetAreaPosition(FieldArea.ColumnArea, index);
                //Fields[field.Name].FilterValues.Clear();
                FieldSetValues(field);
            }
            for (int index = 0; index < layout.RowArea.Count; index++)
            {
                LayoutField field;
                if (layout.RowArea.TryGetValue(index, out field) == false) continue;
                Fields[field.Name].Area = FieldArea.RowArea;
                Fields[field.Name].Visible = true;
                Fields[field.Name].SetAreaPosition(FieldArea.RowArea, index);
                //Fields[field.Name].FilterValues.Clear();
                FieldSetValues(field);
            }
            for (int index = 0; index < layout.DataArea.Count; index++)
            {
                LayoutField field;
                if (layout.DataArea.TryGetValue(index, out field) == false) continue;
                Fields[field.Name].Area = FieldArea.DataArea;
                Fields[field.Name].Visible = true;
                Fields[field.Name].SetAreaPosition(FieldArea.DataArea, index);
                //Fields[field.Name].FilterValues.Clear();
                FieldSetValues(field);
            }
            for (int index = 0; index < layout.FilterArea.Count; index++)
            {
                LayoutField field;
                if (layout.FilterArea.TryGetValue(index, out field) == false) continue;
                Fields[field.Name].Area = FieldArea.FilterArea;
                Fields[field.Name].Visible = true;
                Fields[field.Name].SetAreaPosition(FieldArea.FilterArea, index);
                //Fields[field.Name].FilterValues.Clear();
                FieldSetValues(field);
            }

        }

        private static LayoutField FieldGetValues(PivotGridField pivotGridField)
        {
            return new LayoutField(pivotGridField.FieldName,
                                   pivotGridField.FilterValues.ValuesIncluded.Length <
                                   pivotGridField.FilterValues.ValuesExcluded.Length
                                       ? FilterTypes.Included
                                       : FilterTypes.Excluded,
                                   pivotGridField.FilterValues.ValuesIncluded.Length <
                                   pivotGridField.FilterValues.ValuesExcluded.Length
                                       ? pivotGridField.FilterValues.ValuesIncluded
                                       : pivotGridField.FilterValues.ValuesExcluded);
        }

        public Layout GetLayout()
        {
            var layout = new Layout();

            var fields = GetFieldsByArea(FieldArea.ColumnArea);
            for (int index = 0; index < fields.Count; index++)
            {
                var pivotGridField = fields[index];
                layout.ColumnArea.Add(index, FieldGetValues(pivotGridField));
            }

            fields = GetFieldsByArea(FieldArea.RowArea);
            for (int index = 0; index < fields.Count; index++)
            {
                var pivotGridField = fields[index];
                layout.RowArea.Add(index, FieldGetValues(pivotGridField));
            }

            fields = GetFieldsByArea(FieldArea.DataArea);
            for (int index = 0; index < fields.Count; index++)
            {
                var pivotGridField = fields[index];
                layout.DataArea.Add(index, FieldGetValues(pivotGridField));
            }

            fields = GetFieldsByArea(FieldArea.FilterArea);
            for (int index = 0; index < fields.Count; index++)
            {
                var pivotGridField = fields[index];
                layout.FilterArea.Add(index, FieldGetValues(pivotGridField));
            }

            return layout;
        }
    }
}
