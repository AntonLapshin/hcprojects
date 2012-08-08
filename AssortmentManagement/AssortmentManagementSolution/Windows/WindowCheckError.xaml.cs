using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using AssortmentManagement.Model;
using AssortmentManagement.UserValues;
using DevExpress.Utils;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;

namespace AssortmentManagement.Windows
{
    public partial class WindowCheckError
    {
        private readonly DBManager _db;
        private readonly Check _check;
        private readonly Session _session;
        public event EventHandler WhRestExistsCheckNewDoc;
        public WindowCheckError(Session session, CheckTypes checkType, Check check)
        {
            WhRestExistsCheckNewDoc += WhRestExistsCheckNewDocHandler;
            _session = session;
            _check = check;
            InitializeComponent();
            this.DataContext = check;
            Title = check.Desc;
            if (_check.ProcedureName == "global_wh_rest_exists")
            {
                var menuItemSaveDoc = new MenuItem{Header="Создать документ"};
                menuItemSaveDoc.Click += menuItemSaveDoc_Click;
                menuMain.Items.Add(menuItemSaveDoc);
            }

            Width = 750;
            Height = 650;

            _db = session.GetDbManager();

            #region Initialize Control

            //gridControl1.AutoPopulateColumns = true;
            gridControl1.DataSource = _db.DataTableGet(check.ProcedureName).DefaultView;

            if (check.TableName != null)
            {
                List<Column> columns;

                try
                {

                    columns =
                        _db.GetTableDefinition(check.TableName);
                    /*
                                        columns =
                                            _db.GetTableDefinition(check.TableName ??
                                                                   (checkType == CheckTypes.Local
                                                                        ? "y_assortment_doc_detail"
                                                                        : "y_assortment_united_sec_gtt"));
                    */

                }
                catch (AssortmentException e)
                {
                    MessageBox.Show(e.Message, "Ошибка детализации", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                //var src = new DataTable("SRC");

                foreach (var column in columns)
                {
                    if (column.Name == "PARAMS")
                    {
                        var table = _db.DataTableGet(check.ProcedureName);
                        var maxColumn = 0;
                        /*
                                                foreach (DataColumn col in table.Columns)
                                                {
                                                    if (col.ColumnName != "PARAMS" && col.ColumnName.Contains("PARAM"))table.Columns.Remove(col);
                                                }
                        */


                        for (int i = 1; i < 30; i++)
                        {
                            try
                            {
                                table.Columns.Remove("PARAM" + i);
                            }
                            catch { }
                        }

                        foreach (DataRow row in table.Rows)
                        {
                            var rowParams = row["PARAMS"].ToString();
                            var qty = rowParams.Split(';').Length;
                            if (qty > maxColumn) maxColumn = qty;
                        }
                        for (int i = 1; i <= maxColumn; i++)
                        {
                            table.Columns.Add("PARAM" + i);

                            gridControl1.Columns.Add(new GridColumn
                            {
                                FieldName = "PARAM" + i,
                                Header = "Параметр " + i,
                                AllowEditing = DefaultBoolean.False
                            });
                        }
                        foreach (DataRow row in table.Rows)
                        {
                            var rowParams = row["PARAMS"].ToString();
                            var rowParamsArray = rowParams.Split(';');
                            for (int i = 1; i <= rowParamsArray.Length; i++)
                            {
                                row["PARAM" + i] = rowParamsArray[i - 1];
                            }
                        }
                    }
                    else
                    {
                        //src.Columns.Add(new DataColumn(column.Name));
                        gridControl1.Columns.Add(new GridColumn
                                                     {
                                                         FieldName = column.Name,
                                                         Header = column.Desc,
                                                         AllowEditing = DefaultBoolean.False
                                                     });
                    }
                }
            }
            else
            {

                switch (checkType)
                {
                    case CheckTypes.Local:
                        {
                            gridControl1.Columns.Add(new GridColumn { FieldName = "ITEM", Header = "Товар", AllowEditing = DefaultBoolean.False });
                            gridControl1.Columns.Add(new GridColumn { FieldName = "DIM_ITEM_DESC", Header = "Наименование", AllowEditing = DefaultBoolean.False });
                            gridControl1.Columns.Add(new GridColumn { FieldName = "LOC", Header = "Магазин", AllowEditing = DefaultBoolean.False });
                            gridControl1.Columns.Add(new GridColumn { FieldName = "DIM_LOC_DESC", Header = "Название", AllowEditing = DefaultBoolean.False });
                            gridControl1.Columns.Add(new GridColumn { FieldName = "ACTION", Header = "Действие", AllowEditing = DefaultBoolean.False });
                            gridControl1.Columns.Add(new GridColumn { FieldName = "SUPPLIER", Header = "Поставщик", AllowEditing = DefaultBoolean.False });
                            gridControl1.Columns.Add(new GridColumn { FieldName = "DIM_ITEMLOC_SUPPLIER_DESC", Header = "Название", AllowEditing = DefaultBoolean.False });
                            gridControl1.Columns.Add(new GridColumn { FieldName = "SUPPLIER_NEW", Header = "Поставщик (новый)", AllowEditing = DefaultBoolean.False });
                            gridControl1.Columns.Add(new GridColumn { FieldName = "DIM_ITEMLOC_SUPPLIER_DESC_NEW", Header = "Название", AllowEditing = DefaultBoolean.False });
                            gridControl1.Columns.Add(new GridColumn { FieldName = "ORDERPLACE", Header = "Место заказа", AllowEditing = DefaultBoolean.False });
                            gridControl1.Columns.Add(new GridColumn { FieldName = "ORDERPLACE_NEW", Header = "Место заказа (новое)", AllowEditing = DefaultBoolean.False });
                            gridControl1.Columns.Add(new GridColumn { FieldName = "SOURCEMETHOD", Header = "Тип поставки", AllowEditing = DefaultBoolean.False });
                            gridControl1.Columns.Add(new GridColumn { FieldName = "SOURCEMETHOD_NEW", Header = "Тип поставки (новый)", AllowEditing = DefaultBoolean.False });
                            gridControl1.Columns.Add(new GridColumn { FieldName = "SOURCEWH", Header = "Склад поставки", AllowEditing = DefaultBoolean.False });
                            gridControl1.Columns.Add(new GridColumn { FieldName = "SOURCEWH_NEW", Header = "Склад поставки (новый)", AllowEditing = DefaultBoolean.False });
                            break;
                        }
                    case CheckTypes.Global:
                        {
                            gridControl1.Columns.Add(new GridColumn { FieldName = "ITEM", Header = "Товар", AllowEditing = DefaultBoolean.False });
                            gridControl1.Columns.Add(new GridColumn { FieldName = "DIM_ITEM_DESC", Header = "Наименование", AllowEditing = DefaultBoolean.False });
                            gridControl1.Columns.Add(new GridColumn { FieldName = "LOC", Header = "Магазин", AllowEditing = DefaultBoolean.False });
                            gridControl1.Columns.Add(new GridColumn { FieldName = "DIM_LOC_DESC", Header = "Название", AllowEditing = DefaultBoolean.False });
                            gridControl1.Columns.Add(new GridColumn { FieldName = "DIM_ITEMLOC_SUPPLIER_NEW", Header = "Поставщик", AllowEditing = DefaultBoolean.False });
                            gridControl1.Columns.Add(new GridColumn { FieldName = "DIM_ITEMLOC_SUPPLIER_DESC_NEW", Header = "Имя", AllowEditing = DefaultBoolean.False });
                            gridControl1.Columns.Add(new GridColumn { FieldName = "DIM_ITEMLOC_SOURCEMETHOD_NEW", Header = "Способ поставки", AllowEditing = DefaultBoolean.False });
                            gridControl1.Columns.Add(new GridColumn { FieldName = "DIM_ITEMLOC_SOURCEWH_NEW", Header = "Склад поставки", AllowEditing = DefaultBoolean.False });
                            gridControl1.Columns.Add(new GridColumn { FieldName = "DIM_ITEMLOC_ORDERPLACE_NEW", Header = "Место заказа", AllowEditing = DefaultBoolean.False });
                            gridControl1.Columns.Add(new GridColumn { FieldName = "ACTION", Header = "Действие", AllowEditing = DefaultBoolean.False });

                            break;
                        }
                }

            }

            #endregion
        }

        void WhRestExistsCheckNewDocHandler(object sender, EventArgs e)
        {
            Close();
        }

        void menuItemSaveDoc_Click(object sender, RoutedEventArgs e)
        {
            _db.DataTableUpdateCheckSource(_check.ProcedureName);
            _session.CreateDocument(DocTypes.Regular);
            var winSecondary = new WindowSecondary(_session,isWhRestExistsCheckErrorDoc:true);
            winSecondary.ShowDialog();
            WhRestExistsCheckNewDoc(this, null);
        }

        private void MenuItemExportToXLS(object sender, RoutedEventArgs e)
        {
            //var dlg = new Microsoft.Win32.SaveFileDialog { DefaultExt = ".xlsx", Filter = "Документы Excel (.xlsx)|*.xlsx" };
            //if (dlg.ShowDialog() != true) return;
            //var link = new VisualDataNodeLink((TableView)gridControl1.View, "Список ошибок");
            //link.ExportToXlsx(dlg.FileName);

            MessageBox.Show("Функционал не доступен", "Ошибка", MessageBoxButton.OK);
            /*
            var temp = Path.GetTempFileName() + ".xls";
            var link = new VisualDataNodeLink((TableView) gridControl1.View, _check.Desc);// { PageHeader = (DataTemplate)Resources["PageHeader"] };
            
            var options = new XlsExportOptions { ExportMode = XlsExportMode.SingleFile, SheetName = _check.Desc };
            link.ExportToXls(temp, options);
            Process.Start(temp);
             */
        }
    }
}
