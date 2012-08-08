using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using AssortmentManagement.Model;
using AssortmentManagement.UserValues;
using DevExpress.Utils;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;

namespace AssortmentManagement.Windows
{
    public partial class WindowGrid
    {
        public WindowGrid(string title, List<ItemAddResult> result)
        {
            InitializeComponent();

            Title = title;

            Width = 750;
            Height = 650;

            #region Initialize Control

            //gridControl1.AutoPopulateColumns = true;
            gridControl1.DataSource = result;
            gridControl1.Columns.Add(new GridColumn { FieldName = "Item", Header = "Товар", AllowEditing = DefaultBoolean.False });
            gridControl1.Columns.Add(new GridColumn { FieldName = "ItemDesc", Header = "Название", AllowEditing = DefaultBoolean.False });
            gridControl1.Columns.Add(new GridColumn { FieldName = "Status", Header = "Статус", AllowEditing = DefaultBoolean.False });
            gridControl1.Columns.Add(new GridColumn { FieldName = "StatusRMS", Header = "Статус RMS", AllowEditing = DefaultBoolean.False });
            gridControl1.Columns.Add(new GridColumn { FieldName = "IsManagerItem", Header = "Принадлежность менеджеру", AllowEditing = DefaultBoolean.False });

            #endregion
        }

        private void MenuItemExportToXLS(object sender, RoutedEventArgs e)
        {
            //var dlg = new Microsoft.Win32.SaveFileDialog { DefaultExt = ".xlsx", Filter = "Документы Excel (.xlsx)|*.xlsx" };
            //if (dlg.ShowDialog() != true) return;
            //var link = new VisualDataNodeLink((TableView)gridControl1.View, "Список ошибок");
            //link.ExportToXlsx(dlg.FileName);

            MessageBox.Show("Функционал не доступен", "Ошибка", MessageBoxButton.OK);

            //var temp = Path.GetTempFileName() + ".xls";
            //var link = new VisualDataNodeLink((TableView)gridControl1.View, "Список ошибок");
            //link.ExportToXls(temp); Process.Start(temp);
        }
    }
}
