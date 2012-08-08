using DevExpress.Xpf.Editors;
using NormManagementMVVM.Model;
using SharedComponents.UI;

namespace NormManagementMVVM.Controls
{
    /// <summary>
    /// Логика взаимодействия для RowUnitPanelControl.xaml
    /// </summary>
    public partial class RowUnitPanelControl
    {
        public RowUnitPanelControl()
        {
            InitializeComponent();
        }

        private void MaxColumnValidate(object sender, ValidationEventArgs e)
        {
            var rowControl = UIHelper.FindVisualParent<RowControl>(this);
            if (rowControl == null || e.Value == null) return;
            if ((int.Parse(e.Value.ToString()) >
                 ((Y_NORM_NORMATIVE_ROW) rowControl.DataContext).Y_NORM_NORMATIVE_CELL.Count))
            {
                maxColumn.Text = ((Y_NORM_NORMATIVE_ROW) rowControl.DataContext).Y_NORM_NORMATIVE_CELL.Count.ToString();
            }
            //foreach(var child in rowControl.rowPanel.Children)
            //{
            //    if (child.GetType() != typeof(CellControl)) continue;
            //    var cellControl = child as CellControl;
            //    if (cellControl==null) continue;
            //    if (((Y_NORM_NORMATIVE_CELL) cellControl.DataContext).ID_COLUMN != int.Parse(e.Value.ToString()))
            //        continue;
            //    foreach(var cell in cellControl.CellController.Group)
            //    {
            //        if (cell == cellControl) continue;
            //        ((Y_NORM_NORMATIVE_CELL) cell.DataContext).Y_NORM_NORMATIVE_ROW.MAX_COLUMN =
            //            short.Parse(e.Value.ToString());
            //    }
            //}
        }

        private void MaxColumnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            if (e.OldValue.ToString() == "") return;
            {
                var rowControl = UIHelper.FindVisualParent<RowControl>(this);
                if (rowControl == null) return;
                foreach (object child in rowControl.rowPanel.Children)
                {
                    if (child.GetType() != typeof (CellControl)) continue;
                    var cellControl = child as CellControl;
                    if (cellControl == null) continue;
                    if (((Y_NORM_NORMATIVE_CELL) cellControl.DataContext).ID_COLUMN != int.Parse(e.NewValue.ToString()))
                        continue;
                    foreach (CellControl cell in cellControl.CellController.Group)
                    {
                        if (cell == cellControl) continue;
                        ((Y_NORM_NORMATIVE_CELL) cell.DataContext).Y_NORM_NORMATIVE_ROW.MAX_COLUMN =
                            short.Parse(e.NewValue.ToString());
                    }
                }
            }
        }
    }
}