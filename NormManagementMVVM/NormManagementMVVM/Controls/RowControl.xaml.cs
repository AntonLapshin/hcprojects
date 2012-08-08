using System;
using System.Linq;
using System.Windows;
using NormManagement.Controls;
using NormManagementMVVM.Model;

namespace NormManagementMVVM.Controls
{
    /// <summary>
    /// Логика взаимодействия для RowControl.xaml
    /// </summary>
    public partial class RowControl
    {
        public RowControl()
        {
            InitializeComponent();
            var btnPlus = new PlusControl();
            btnPlus.PlusClick += BtnPlusPlusClick;
            rowPanel.Children.Add(btnPlus);
            DataContextChanged += RowControlDataContextChanged;
        }

        private void BtnPlusPlusClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (rowPanel.Children[rowPanel.Children.Count - 2].GetType() == typeof (CellControl))
                {
                    var currentCellControl = (CellControl) rowPanel.Children[rowPanel.Children.Count - 2];
                    if (((Y_NORM_NORMATIVE_CELL) currentCellControl.DataContext).ID_PARAM == 0 ||
                        ((Y_NORM_NORMATIVE_CELL) currentCellControl.DataContext).PARAM_VALUE == null)
                    {
                        MessageBox.Show("Заполните предыдущую ячейку", "Ошибка", MessageBoxButton.OK,
                                        MessageBoxImage.Warning);
                        return;
                    }
                }
                var cell = new Y_NORM_NORMATIVE_CELL
                               {
                                   ID_COLUMN =
                                       IdGenerator.GetId(((Y_NORM_NORMATIVE_ROW) DataContext).Y_NORM_NORMATIVE_CELL)
                               };
                ((Y_NORM_NORMATIVE_ROW) DataContext).Y_NORM_NORMATIVE_CELL.Add(cell);
                ((Y_NORM_NORMATIVE_ROW) DataContext).MAX_COLUMN =
                    (short) ((Y_NORM_NORMATIVE_ROW) DataContext).Y_NORM_NORMATIVE_CELL.Count;
                var cellControl = new CellControl {DataContext = cell};
                if (rowPanel.Children[rowPanel.Children.Count - 2].GetType() == typeof (CellControl))
                {
                    ((CellControl) rowPanel.Children[rowPanel.Children.Count - 2]).CellController.IsUnLocked = false;
                }

                rowPanel.Children.Insert(rowPanel.Children.Count - 1, cellControl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RowControlDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var rowDataContext = e.NewValue as Y_NORM_NORMATIVE_ROW;

            foreach (Y_NORM_NORMATIVE_CELL cell in rowDataContext.Y_NORM_NORMATIVE_CELL.OrderBy(i => i.ID_COLUMN))
            {
                var cellControl = new CellControl {DataContext = cell};

                rowPanel.Children.Add(cellControl);
            }

            rowPanel.Children.Add(new RowUnitPanelControl {DataContext = DataContext, Name = "RowUnit"});
        }
    }
}