using System;
using System.Windows;
using System.Windows.Input;
using AssortmentManagement.Model;
using DevExpress.Utils;
using DevExpress.Xpf.Grid;

namespace AssortmentManagement.Windows
{
    

    public partial class WindowSupplier
    {
        private readonly DBManager _db;        
        private System.Data.DataView dv;
        public delegate void SupplierEventHandler(object sender, SupplierEventArgs e);
        public event SupplierEventHandler SupplierSelected;
        private string filterColumn = "SUPPLIER";
    
        public WindowSupplier(Object dbObject)
        {
            InitializeComponent();

            Width = 600;
            Height = 650;            

            _db = (DBManager)dbObject;
            dv = _db.DataTableGet("sups").DefaultView;
            dv.RowFilter = null;


            #region Initialize Control

            gridControl1.DataSource = dv;

            gridControl1.Columns.Add(new GridColumn { FieldName = "SUPPLIER", Header="Код", AllowEditing = DefaultBoolean.False });
            gridControl1.Columns.Add(new GridColumn { FieldName = "SUP_NAME", Header="Наименование", Width = 400, AllowEditing = DefaultBoolean.False });

            //gridControl1.View.FilterEditorCreated += ViewFilterEditorCreated;
            gridControl1.MouseDoubleClick += GridControl1MouseDoubleClick;

            lblFilter.Text = "Фильтр по коду:";
            tbFilter.Focus();

            #endregion
        }

        #region Event Handlers

        private void GridControl1MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SupplierSelected(this, new SupplierEventArgs
                                                    {
                                                        Supplier = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("SUPPLIER")),
                                                        Name = (string)gridControl1.GetFocusedRowCellValue("SUP_NAME")
                                                    });
            Close();
        }
        //private void ViewFilterEditorCreated(object sender, FilterEditorEventArgs e)
        //{
        //    e.FilterControl.ShowGroupCommandsIcon = filterEditor.ShowGroupCommandsIcon;
        //    e.FilterControl.ShowOperandTypeIcon = filterEditor.ShowOperandTypeIcon;
        //    e.FilterControl.ShowToolTips = filterEditor.ShowToolTips;
        //}
        //private void BtnApplyClick(object sender, RoutedEventArgs e)
        //{
        //    filterEditor.ApplyFilter();
        //}

        #endregion        

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.F))
            {
                lblFilter.Text = "Фильтр по коду:";
                tbFilter.Focus();
            }            
        }

        private void tbFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            dv.RowFilter = null;
            try
            {
                if (filterColumn == "SUPPLIER")
                    dv.RowFilter = filterColumn + " = " + tbFilter.Text;
                else
                    dv.RowFilter = filterColumn + " LIKE '%" + tbFilter.Text + "%'";
            }
            catch
            { }
        }

        private void gridControl1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            cvFilter.Visibility = Visibility.Visible;
            try
            {
                filterColumn = gridControl1.View.GetColumnByMouseEventArgs(e).FieldName;                 
            }
            catch (Exception)
            {               
                filterColumn = "SUPPLIER";
            }
            if (filterColumn == "SUPPLIER")
                lblFilter.Text = "Фильтр по коду:";
            else lblFilter.Text = "Фильтр по наименованию:";
            tbFilter.Focus();
        }
    }


}
