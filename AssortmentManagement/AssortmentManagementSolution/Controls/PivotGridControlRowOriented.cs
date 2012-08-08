using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AssortmentManagement.Model;
using AssortmentManagement.UserValues;
using DevExpress.Xpf.PivotGrid;

namespace AssortmentManagement.Controls
{
    public class PivotGridControlRowOriented : PivotGridControl
    {
        #region Fields

        public delegate void CellInputDataEventHandler(object sender, CellInputDataEventArgs e);
        public event CellInputDataEventHandler CellClickInputData;
        public CellInputDataEventArgs CellArgs { get; set; }
        private DBManager _db;
        #endregion

        public void InitializeControl(List<Column> dimensions, Object db)
        {
            _db = (DBManager)db;
            InitializeFields(dimensions);
            //SetFieldListSize(Size.Empty, new Size(250, 600));

            CellArgs = new CellInputDataEventArgs();

            #region Initialize Event Handlers

            FieldAreaChanging += PivotGridControlModifiedFieldAreaChanging;
            FieldFilterChanged += PivotGridControlModifiedFieldFilterChanged;
            CustomSummary += PivotGridControlRowOrientedCustomSummary;
            CellClick += PivotGridControlRowOrientedCellClick;

            #endregion
        }
        public void InitializeLayout(PivotGridControl control)
        {
            //Fields["ITEM"].Area = FieldArea.RowArea;

            //foreach (var field in control.GetFieldsByArea(FieldArea.RowArea))
            //{
            //    if (field.FieldName.Contains("ITEMLOC")) continue;
            //    PivotGridField f = Fields[field.FieldName];
            //    f.Visible = true;
            //    f.Area = FieldArea.RowArea;
            //}

            // remove -1 (delete action) from the action field and it lock
            foreach (object t in Fields["ACTION"].FilterValues.ValuesIncluded)
            {
                if (Convert.ToInt32(t) == -1)
                {
                    Fields["ACTION"].FilterValues.ValuesExcluded = new[] { t };
                }
            }
/*            
            foreach (object t in Fields["DIM_LOC_TYPE"].FilterValues.ValuesIncluded)
            {
                if (Convert.ToChar(t) == (char)LocTypes.W)
                {
                    Fields["DIM_LOC_TYPE"].FilterValues.ValuesExcluded = new[] { t };
                }
            }
*/            

            Fields["ACTION"].Area = FieldArea.RowArea;
            Fields["ACTION"].Visible = true;
            Fields["ITEM"].Area = FieldArea.RowArea;
            Fields["ITEM"].Visible = true;
            Fields["DIM_ITEM_DESC"].Area = FieldArea.RowArea;
            Fields["DIM_ITEM_DESC"].Visible = true;
            Fields["DIM_LOC_TYPE"].Area = FieldArea.FilterArea;
            Fields["DIM_LOC_TYPE"].Visible = true;
            Fields["DIM_LOC_TYPE"].AllowDrag = false;
            //Fields["DIM_LOC_TYPE"].AllowFilter = false;
            Fields["DIM_LOC_CHAIN"].Area = FieldArea.FilterArea;
            Fields["DIM_LOC_CHAIN"].Visible = true;
            Fields["DIM_LOC_REGION"].Area = FieldArea.FilterArea;
            Fields["DIM_LOC_REGION"].Visible = true;
            Fields["DIM_LOC_FORMAT"].Area = FieldArea.FilterArea;
            Fields["DIM_LOC_FORMAT"].Visible = true;
            Fields["DIM_LOC_STANDARD"].Area = FieldArea.FilterArea;
            Fields["DIM_LOC_STANDARD"].Visible = true;
            Fields["LOC"].Area = FieldArea.FilterArea;
            Fields["LOC"].Visible = true;
            Fields["DIM_LOC_DESC"].Area = FieldArea.FilterArea;
            Fields["DIM_LOC_DESC"].Visible = true;

            Fields["DIM_ITEMLOC_SUPPLIER"].Area = FieldArea.DataArea;
            Fields["DIM_ITEMLOC_SUPPLIER_DESC"].Area = FieldArea.DataArea;

            Fields["DIM_ITEMLOC_SUPPLIER_NEW"].Area = FieldArea.DataArea;
            Fields["DIM_ITEMLOC_SUPPLIER_NEW"].Caption = "(NEW)Поставщик";
            Fields["DIM_ITEMLOC_SUPPLIER_DESC_NEW"].Area = FieldArea.DataArea;
            Fields["DIM_ITEMLOC_SUPPLIER_DESC_NEW"].Caption = "(NEW)Поставщик.Название";

            Fields["DIM_ITEMLOC_ORDERPLACE"].Area = FieldArea.DataArea;
            Fields["DIM_ITEMLOC_ORDERPLACE_NEW"].Area = FieldArea.DataArea;
            Fields["DIM_ITEMLOC_ORDERPLACE_NEW"].Caption = "(NEW)Место заказа";

            Fields["DIM_ITEMLOC_SOURCEMETHOD"].Area = FieldArea.DataArea;
            Fields["DIM_ITEMLOC_SOURCEWH"].Area = FieldArea.DataArea;

            Fields["DIM_ITEMLOC_SOURCEMETHOD_NEW"].Area = FieldArea.DataArea;
            Fields["DIM_ITEMLOC_SOURCEMETHOD_NEW"].Caption = "(NEW)Метод поставки";
            Fields["DIM_ITEMLOC_SOURCEWH_NEW"].Area = FieldArea.DataArea;
            Fields["DIM_ITEMLOC_SOURCEWH_NEW"].Caption = "(NEW)Склад поставки";

            Fields["ITEM"].FilterValues.ValuesIncluded = control.Fields["ITEM"].GetVisibleValues().ToArray();
            Fields["LOC"].FilterValues.ValuesIncluded = control.Fields["LOC"].GetVisibleValues().ToArray();
            Fields["DIM_ITEMLOC_SUPPLIER"].Visible = true;
            Fields["DIM_ITEMLOC_SUPPLIER_DESC"].Visible = true;
            Fields["DIM_ITEMLOC_SUPPLIER_NEW"].Visible = true;
            Fields["DIM_ITEMLOC_SUPPLIER_DESC_NEW"].Visible = true;
            Fields["DIM_ITEMLOC_ORDERPLACE"].Visible = true;
            Fields["DIM_ITEMLOC_ORDERPLACE_NEW"].Visible = true;
            Fields["DIM_ITEMLOC_SOURCEMETHOD"].Visible = true;
            Fields["DIM_ITEMLOC_SOURCEWH"].Visible = true;
            Fields["DIM_ITEMLOC_SOURCEMETHOD_NEW"].Visible = true;
            Fields["DIM_ITEMLOC_SOURCEWH_NEW"].Visible = true;
        }

        #region Event Handlers

        private void PivotGridControlRowOrientedCellClick(object sender, PivotCellEventArgs e)
        {
            if (e.RowField == null) return;
            
            #region Initialize Condition

            var fc = e.GetColumnFields();
            var fr = e.GetRowFields();

            var fieldValues = fc.Select(pivotGridField => new FieldValue { Field = pivotGridField.FieldName, Value = e.GetFieldValue(pivotGridField) }).ToList();
            fieldValues.AddRange(fr.Select(pivotGridField => new FieldValue { Field = pivotGridField.FieldName, Value = e.GetFieldValue(pivotGridField) }));

            CellArgs = new CellInputDataEventArgs();
            CellArgs.ConditionValues = fieldValues;

            #endregion
            
            #region Initialize Filters

            //var fields = GetFieldsByArea(FieldArea.FilterArea);
            //var filters = new List<FilterValues>();

            //foreach (var field in fields)
            //{
            //    filters.Add(new FilterValues { Field = field.FieldName, Values = field.FilterValues.ValuesIncluded.ToList(), ShowBlanks = field.FilterValues.ShowBlanks });
            //}

            var filters = (from field in Fields where field.Visible && field.FilterValues.HasFilter select new FilterValues { Field = field.FieldName, Values = field.FilterValues.ValuesIncluded.ToList(), ShowBlanks = field.FilterValues.ShowBlanks }).ToList();
            
            CellArgs.Filters = filters;

            #endregion

            #region Calculate Condition Locs
/*
            var setIL = _db.DataTableGetILByCondition(Table.TableRowSource, CellArgs.ConditionValues,
                                              CellArgs.Filters);
            var locs = _db.DataTableGetLocsByIL(setIL);
*/
            var userWhList = _db.UserWhList.Keys.ToList();
/*
            foreach (var loc in locs)
            {
                if (userWhList.Contains(loc))
                {
                    userWhList.Remove(loc);
                }
            }

            var items = _db.DataTableGetItemsByIL(setIL);

            foreach (var item in items)
            {
                var whs = _db.DataTableGetWhRowsByItem(item);
                foreach (var wh in whs)
                {
                    if (userWhList.Contains(wh))userWhList.Remove(wh);
                }
            }
*/
            #endregion

            #region Input Data Types

            //CellArgs.FieldName = e.DataField.FieldName;
            //CellArgs.DataTypeName = e.DataField.DataType.Name;))

            if (e.DataField == Fields["DIM_ITEMLOC_SUPPLIER_NEW"] || e.DataField == Fields["DIM_ITEMLOC_SUPPLIER_DESC_NEW"])
            {
                CellArgs.SetValues.Add(new FieldValue { Field = "DIM_ITEMLOC_SUPPLIER_NEW", Value = null });
                CellArgs.SetValues.Add(new FieldValue { Field = "DIM_ITEMLOC_SUPPLIER_DESC_NEW", Value = null });
                CellArgs.Type = InputDataTypes.Supplier;
                CellClickInputData(this, CellArgs);
            }
            else if (e.DataField == Fields["DIM_ITEMLOC_ORDERPLACE_NEW"])
            {
                CellArgs.SetValues.Add(new FieldValue { Field = e.DataField.FieldName, Value = null });
                CellArgs.Type = InputDataTypes.OrderPlace;

                #region Context Menu
                var menu = new ContextMenu();
                for (var i = OrderPlaces.Supplier; i < OrderPlaces.Office+1; i++)
                {
                    var menuItem = new MenuItem { Tag = (int)i, Header = i.Description() };
                    menuItem.Click += ContextMenuItemClick;
                    menu.Items.Add(menuItem);
                }

                //var menuItemOrderPlace1 = new MenuItem { Tag = (int)OrderPlace.Supplier, Header = OrderPlace.Supplier.Description() };
                //menuItemOrderPlace1.Click += ContextMenuItemOrderPlaceClick;

                //var menuItemOrderPlace2 = new MenuItem { Tag = (int)OrderPlace.Store, Header = OrderPlace.Store.Description() };
                //menuItemOrderPlace2.Click += ContextMenuItemOrderPlaceClick;

                //var menuItemOrderPlace3 = new MenuItem { Tag = (int)OrderPlace.Office, Header = OrderPlace.Office.Description() };
                //menuItemOrderPlace3.Click += ContextMenuItemOrderPlaceClick;

                //var menu = new ContextMenu();
                //menu.Items.Add(menuItemOrderPlace1);
                //menu.Items.Add(menuItemOrderPlace2);
                //menu.Items.Add(menuItemOrderPlace3);
                menu.IsOpen = true;

                #endregion
            }
            else if (e.DataField == Fields["DIM_ITEMLOC_SOURCEMETHOD_NEW"])
            {
                CellArgs.SetValues.Add(new FieldValue { Field = e.DataField.FieldName, Value = null });
                CellArgs.SetValues.Add(new FieldValue { Field = "DIM_ITEMLOC_SOURCEWH_NEW", Value = null });
                CellArgs.Type = InputDataTypes.SourceMethod;

                #region Context Menu
                var menuItemSourceMethod = new MenuItem { Tag = (char)SourceMethods.S, Header = SourceMethods.S.Description() };
                menuItemSourceMethod.Click += ContextMenuItemClick;
                var menu = new ContextMenu();
                menu.Items.Add(menuItemSourceMethod);

                //if (_db.UserWhList.Count != 0)
                if (userWhList.Count != 0)
                {
                    var menuItemSourceMethod2 = new MenuItem { Tag = (char)SourceMethods.W, Header = SourceMethods.W.Description() };
                    menuItemSourceMethod2.Click += ContextMenuItemClick;

                    var menuItemSourceMethod3 = new MenuItem { Tag = (char)SourceMethods.T, Header = SourceMethods.T.Description() };
                    menuItemSourceMethod3.Click += ContextMenuItemClick;
                    
                    menu.Items.Add(menuItemSourceMethod2);
                    menu.Items.Add(menuItemSourceMethod3);
                    menu.IsOpen = true;
                }

                menu.IsOpen = true;
                #endregion
            }
            else if (e.DataField == Fields["DIM_ITEMLOC_SOURCEWH_NEW"])
            {
                #region Check Source Method

                var r = e.GetRowFields();
                var sourcemethod = e.GetCellValue(null, r.Select(f => e.GetFieldValue(f)).ToArray(), Fields["DIM_ITEMLOC_SOURCEMETHOD_NEW"]) as CellRowOriented;
                if (sourcemethod == null) return;
                if (sourcemethod.Value == "" || 
                    Convert.ToChar(sourcemethod.Value) == (char)SourceMethods.S ||
                    sourcemethod.Value == "?") return;

                #endregion

                CellArgs.SetValues.Add(new FieldValue { Field = e.DataField.FieldName, Value = null});
                
                CellArgs.Type = InputDataTypes.SourceWh;

                #region Context Menu
                
                var menu = new ContextMenu();
                foreach (var wh in userWhList)
                {
                    //var menuItemSource = new MenuItem { Header = wh.Key, Tag = wh.Key };
                    var menuItemSource = new MenuItem { Header = wh, Tag = wh };
                    menuItemSource.Click += ContextMenuItemClick;
                    menu.Items.Add(menuItemSource);
                }
                menu.IsOpen = true;

                #endregion
            }
            else
            {
                return;
            }

            #endregion
        }
        private void ContextMenuItemClick(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            CellArgs.SetValues[0].Value = menuItem.Tag;
            CellClickInputData(this, CellArgs);
        }

        //private void ContextMenuItemSourceClick(object sender, RoutedEventArgs e)
        //{
        //    var sourceWh = sender as MenuItem;
        //    CellArgs.SetValues[0].Value = sourceWh.Header;
        //    CellClickInputData(this, CellArgs);
        //}

        //private void ContextMenuItemSourceMethodClick(object sender, RoutedEventArgs e)
        //{
        //    var sourceMethod = sender as MenuItem;
        //    CellArgs.SetValues[0].Value = sourceMethod.Tag;
        //    CellClickInputData(this, CellArgs);
        //}

        //private void ContextMenuItemOrderPlaceClick(object sender, RoutedEventArgs e)
        //{
        //    var orderPlace = sender as MenuItem;
        //    CellArgs.SetValues[0].Value = orderPlace.Tag;
        //    CellClickInputData(this, CellArgs);
        //}

        private static void PivotGridControlRowOrientedCustomSummary(object sender, PivotCustomSummaryEventArgs e)
        {
            if (e.FieldName.Contains("NEW"))
            {
                bool isNull = false;
                var list = e.CreateDrillDownDataSource();
                for (int index = 0; index < list.RowCount; index++)
                {
                    PivotDrillDownDataRow row = list[index];
                    if (isNull == false)
                    {
                        if (e.FieldName.Equals("DIM_ITEMLOC_SOURCEWH_NEW"))
                        {
                            if (row["DIM_ITEMLOC_SOURCEMETHOD_NEW"] == null)
                            {
                                isNull = true;
                            }
                            else if ((Convert.ToChar(row["DIM_ITEMLOC_SOURCEMETHOD_NEW"]) == (char)SourceMethods.W || Convert.ToChar(row["DIM_ITEMLOC_SOURCEMETHOD_NEW"]) == (char)SourceMethods.T) && row["DIM_ITEMLOC_SOURCEWH_NEW"] == null)
                            {
                                isNull = true;
                            }
                        }
                        else if (row[e.FieldName] == null)
                        {
                            isNull = true;
                        }
                    }
                }

                object val = e.SummaryValue.Min != e.SummaryValue.Max ? "?" : e.SummaryValue.Max;

                CellRowOriented cellValue = new CellRowOriented
                                                {
                                                    Value = (val == null ? "" : Convert.ToString(val)),
                                                    IsNull = isNull
                                                };
                //e.CustomValue = e.SummaryValue.Min != e.SummaryValue.Max ? "xxx" : e.SummaryValue.Max;
                e.CustomValue = cellValue;
            }
            else
            {
                e.CustomValue = e.SummaryValue.Min != e.SummaryValue.Max ? "?" : e.SummaryValue.Max;
            }
        }
        private void PivotGridControlModifiedFieldFilterChanged(object sender, PivotFieldEventArgs e)
        {
            if (e.Field.FilterValues.ValuesExcluded.Count() == 0)
            {
                e.Field.AllowDrag = true;
                e.Field.HeaderTemplate = FieldHeaderTemplate;
            }
            else
            {
                e.Field.AllowDrag = false;
                e.Field.HeaderTemplate = (DataTemplate)FindResource("ItemTemplate");
            }
        }
        private static void PivotGridControlModifiedFieldAreaChanging(object sender, PivotFieldAreaChangingEventArgs e)
        {
            //if (e.Field.FieldName.Contains("SUPPLIER") || e.Field.FieldName.Contains("SOURCEMETHOD") || e.Field.FieldName.Contains("ORDERPLACE") || e.Field.FieldName.Contains("SOURCEWH"))
            if (e.Field.FieldName.Contains("NEW"))
            {
                if (e.NewArea == FieldArea.RowArea || e.NewArea == FieldArea.ColumnArea || e.NewArea == FieldArea.FilterArea)
                {
                    e.Allow = false;
                }
            }
            else if (e.Field.FieldName.Contains("ITEMLOC"))
            {
                //if (e.NewArea == FieldArea.DataArea || e.NewArea == FieldArea.ColumnArea)
                //{
                //    e.Allow = false;
                //}
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
                if (e.NewArea == FieldArea.DataArea || e.NewArea == FieldArea.ColumnArea)
                {
                    e.Allow = false;
                }
            }
            else if (e.Field.FieldName.Contains("MEASURE"))
            {
                e.Allow = false;
            }
        }

        #endregion

        public string GetCondition(List<FieldArea> areas)
        {
            return areas.Select(GetConditionSpecial).Where(result => result.Length != 0).Aggregate("", (current, result) => current + result);
        }

        private void InitializeFields(IEnumerable<Column> dimensions)
        {
            foreach (var dimension in dimensions)
            {
                if (dimension.Name == "DIM_ITEMLOC_TRANSITWH" || dimension.Name == "DIM_ITEMLOC_STATUS_OLD") continue;
                var field = new PivotGridField { FieldName = dimension.Name, Caption = dimension.Desc, Visible = false };
                if (dimension.Name.Contains("ITEMLOC"))
                {
                    field.CellFormat = "{0}";
                    field.SummaryType = FieldSummaryType.Custom;
                    if (dimension.Name.Contains("NEW"))
                        field.CellTemplate = (DataTemplate)FindResource("CellTemplateRowOriented");
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
                    field.FilterValues.ShowBlanks = true;
                    field.CellFormat = "{0}";
                    field.SummaryType = FieldSummaryType.Sum;
                }
                Fields.Add(field);
            }
        }
        private string GetConditionSpecial(FieldArea area)
        {
            string condition = "";

            var fields = GetFieldsByArea(area);
            foreach (var field in fields)
            {
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
    }
}
