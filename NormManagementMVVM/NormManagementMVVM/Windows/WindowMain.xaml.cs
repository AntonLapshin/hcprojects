using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using DevExpress.Xpf.PivotGrid;
using DevExpress.Xpf.PivotGrid.Printing;
using DevExpress.XtraPivotGrid.Localization;
using NormManagementMVVM.Model;
using SharedComponents.Localization;

namespace NormManagementMVVM.Windows
{
    /// <summary>
    /// Логика взаимодействия для WindowMain.xaml
    /// </summary>
    public partial class WindowMain
    {
        private List<PivotRow> _collection;
        private DataTable _dataTable;
        private int _maxItemParam;
        private int _maxStoreParam;

        public WindowMain()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void InitializePivotGrid()
        {
            PivotGridLocalizer.Active = new DXPivotGridLocalizerRU();
            normPivotGridControl.DataSource = _dataTable;
            normPivotGridControl.FieldFilterChanged += PivotGridControlModifiedFieldFilterChanged;
            FillPivotGrid();
            normPivotGridControl.ShowRowTotals = false;
            normPivotGridControl.ShowColumnTotals = false;
            normPivotGridControl.PrintInsertPageBreaks = false;
            normPivotGridControl.PrintLayoutMode = PrintLayoutMode.SinglePageLayout;
        }

        private void InitializeDataTable()
        {
            _collection = GenericRepository.GetPivotRows();
            _dataTable = new DataTable("PivotParams");
            GetMaxLengthStoreItemParams();
            CreateDataTableFields();
            FillDataTable();
        }

        private void GetMaxLengthStoreItemParams()
        {
            foreach (PivotRow pivotRow in _collection)
            {
                int qtyItem = pivotRow.ItemParams == "" ? 0 : pivotRow.ItemParams.Split(';').Length;
                int qtyStore = pivotRow.StoreParams == "" ? 0 : pivotRow.StoreParams.Split(';').Length;
                if (qtyItem > _maxItemParam) _maxItemParam = qtyItem;
                if (qtyStore > _maxStoreParam) _maxStoreParam = qtyStore;
            }
        }

        private void CreateDataTableFields()
        {
            for (int i = 1; i <= _maxStoreParam; i++)
            {
                //normPivotGridControl.Fields.Add(new PivotGridField
                //                                  {FieldName = "StoreParam" + i, Caption = "Параметр магазина" + i,Area = FieldArea.ColumnArea, Visible = true});
                _dataTable.Columns.Add(new DataColumn {Caption = "Параметр магазина" + i, ColumnName = "StoreParam" + i});
            }
            _dataTable.Columns.Add(new DataColumn {ColumnName = "Section", Caption = "Секция"});
            _dataTable.Columns.Add(new DataColumn {ColumnName = "Profile", Caption = "Профиль"});
            for (int i = 1; i <= _maxItemParam; i++)
            {
                //normPivotGridControl.Fields.Add(new PivotGridField { FieldName = "ItemParam" + i, Caption = "Параметр товара" + i, Area = FieldArea.RowArea, Visible = true});
                _dataTable.Columns.Add(new DataColumn {ColumnName = "ItemParam" + i, Caption = "Параметр товара" + i});
            }
            _dataTable.Columns.Add(new DataColumn
                                       {
                                           ColumnName = "Delta",
                                           Caption = "НеАссПлан",
                                           DataType = Type.GetType("System.Int32")
                                       });
            _dataTable.Columns.Add(new DataColumn
                                       {
                                           ColumnName = "Sku",
                                           Caption = "АссПлан",
                                           DataType = Type.GetType("System.Int32")
                                       });
            _dataTable.Columns.Add(new DataColumn
                                       {
                                           ColumnName = "DeltaMin",
                                           Caption = "НеАссФактМин",
                                           DataType = Type.GetType("System.Int32")
                                       });
            _dataTable.Columns.Add(new DataColumn
                                       {
                                           ColumnName = "DeltaMax",
                                           Caption = "НеАссФактМакс",
                                           DataType = Type.GetType("System.Int32")
                                       });
            _dataTable.Columns.Add(new DataColumn
                                       {
                                           ColumnName = "SkuMin",
                                           Caption = "АссФактМин",
                                           DataType = Type.GetType("System.Int32")
                                       });
            _dataTable.Columns.Add(new DataColumn
                                       {
                                           ColumnName = "SkuMax",
                                           Caption = "АссФактМакс",
                                           DataType = Type.GetType("System.Int32")
                                       });
        }

        private void FillDataTable()
        {
            foreach (PivotRow pivotRow in _collection)
            {
                var dataRow = new List<object>();
                if (_maxStoreParam != 0)
                {
                    string[] storeParams = pivotRow.StoreParams.Split(';');
                    dataRow.AddRange(storeParams);
                    if (storeParams.Length != _maxStoreParam)
                    {
                        for (int i = 0; i < _maxStoreParam - storeParams.Length; i++)
                        {
                            dataRow.Add(null);
                        }
                    }
                }
                dataRow.Add(pivotRow.Section);
                dataRow.Add(pivotRow.Profile);
                if (_maxItemParam != 0)
                {
                    string[] itemParams = pivotRow.ItemParams.Split(';');
                    dataRow.AddRange(itemParams);
                    if (itemParams.Length != _maxItemParam)
                    {
                        for (int i = 0; i < _maxItemParam - itemParams.Length; i++)
                        {
                            dataRow.Add(null);
                        }
                    }
                }
                dataRow.Add(pivotRow.Delta);
                dataRow.Add(pivotRow.Sku);
                dataRow.Add(pivotRow.DeltaMin);
                dataRow.Add(pivotRow.DeltaMax);
                dataRow.Add(pivotRow.SkuMin);
                dataRow.Add(pivotRow.SkuMax);
                _dataTable.Rows.Add(dataRow.ToArray());
            }
        }

        private void FillPivotGrid()
        {
            foreach (object column in _dataTable.Columns)
            {
                var dtColumn = column as DataColumn;
                if (dtColumn != null)
                {
                    var field = new PivotGridField
                                    {
                                        FieldName = dtColumn.ColumnName,
                                        Caption = dtColumn.Caption,
                                        Visible = true
                                    };

                    if (dtColumn.ColumnName.Contains("Store"))
                    {
                        field.Area = FieldArea.ColumnArea;
                    }
                    else if (dtColumn.ColumnName.Equals("Section") || dtColumn.ColumnName.Equals("Profile") ||
                             dtColumn.ColumnName.Contains("Item"))
                    {
                        field.Area = FieldArea.RowArea;
                    }

                    else
                    {
                        field.Area = FieldArea.DataArea;
                        field.SummaryType = FieldSummaryType.Sum;
                        field.CellFormat = "{0}";
                    }
                    normPivotGridControl.Fields.Add(field);
                }
            }
        }

        private void FillClick(object sender, RoutedEventArgs e)
        {
            var normView = new View.NormView();
            normView.ShowDialog();
            //var winNorm = new WindowNorm();
            //winNorm.ShowDialog();
            //if (winNorm.IsSaved) RefreshData();
        }

        private void MenuItemFieldsListClick(object sender, RoutedEventArgs e)
        {
            if (normPivotGridControl.IsFieldListVisible)
            {
                menuItemFieldsList.Opacity = 0.5;
                normPivotGridControl.HideFieldList();
            }
            else
            {
                menuItemFieldsList.Opacity = 1;
                normPivotGridControl.ShowFieldList();
            }
        }

        private void MenuItemSummaryClick(object sender, RoutedEventArgs e)
        {
            normPivotGridControl.ShowRowTotals = !normPivotGridControl.ShowRowTotals;
            normPivotGridControl.ShowColumnTotals = !normPivotGridControl.ShowColumnTotals;
        }

        private void MenuItemSummaryTotalClick(object sender, RoutedEventArgs e)
        {
            normPivotGridControl.ShowColumnGrandTotals = !normPivotGridControl.ShowColumnGrandTotals;
            normPivotGridControl.ShowRowGrandTotals = !normPivotGridControl.ShowRowGrandTotals;
        }

        private void PivotGridControlModifiedFieldFilterChanged(object sender, PivotFieldEventArgs e)
        {
            if (e.Field.FilterValues.HasFilter == false)

            {
                e.Field.HeaderTemplate = normPivotGridControl.FieldHeaderTemplate;
                e.Field.HeaderListTemplate = normPivotGridControl.FieldHeaderListTemplate;
            }
            else
            {
                e.Field.HeaderTemplate = (DataTemplate) FindResource("PivotGridFilterTemplate");
                e.Field.HeaderListTemplate = (DataTemplate) FindResource("PivotGridFilterTemplate");
            }
        }

        private void MenuItemExportClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string temp = Path.GetTempFileName() + ".xlsx";
                normPivotGridControl.ExportToXlsx(temp);
                Process.Start(temp);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при выгрузке", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            //var link = new VisualDataNodeLink((TableView)gridControl1.View, "Список ошибок");
            //link.ExportToXls(temp); Process.Start(temp);
        }

        private void MenuItemEquipClick(object sender, RoutedEventArgs e)
        {
            var win = new WindowEquipStore();
            win.ShowDialog();
            if (win.IsSaved) RefreshData();
        }

        private void MenuItemProfileClick(object sender, RoutedEventArgs e)
        {
            var win = new WindowProfile();
            win.ShowDialog();
            if (win.IsSaved) RefreshData();
        }

        private void MenuItemEquipTypeClick(object sender, RoutedEventArgs e)
        {
            var win = new WindowEquipType();
            win.ShowDialog();
            //if (win.IsSaved) RefreshData();
        }

        private void MenuItemParametersClick(object sender, RoutedEventArgs e)
        {
            var win = new WindowParameters();
            win.ShowDialog();
            if (win.IsSaved) RefreshData();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                GenericRepository.InitializeServer();
                InitializeDataTable();
                InitializePivotGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MenuItemRefreshDataClick(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {
            InitializeDataTable();
            normPivotGridControl.Fields.Clear();
            InitializePivotGrid();
        }
    }
}