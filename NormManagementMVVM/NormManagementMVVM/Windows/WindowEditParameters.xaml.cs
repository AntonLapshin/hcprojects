using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using NormManagementMVVM.Model;

namespace NormManagementMVVM.Windows
{
    /// <summary>
    /// Логика взаимодействия для WindowParameters.xaml
    /// </summary>
    public partial class WindowEditParameters
    {
        public List<Y_NORM_PARAMETERS> Parameters;

        public WindowEditParameters(Y_NORM_NORMATIVE_CELL currentCell)
        {
            InitializeComponent();
            Parameters = GenericRepository.GetAllList<Y_NORM_PARAMETERS>();
            foreach (Y_NORM_NORMATIVE_CELL cell in currentCell.Y_NORM_NORMATIVE_ROW.Y_NORM_NORMATIVE_CELL)
            {
                Parameters.Remove(cell.Y_NORM_PARAMETERS);
            }
            paramGridControl.ItemsSource = Parameters;
        }

        public WindowEditParameters()
        {
            InitializeComponent();
            Parameters =
                GenericRepository.GetAllList<Y_NORM_PARAMETERS>().Where(
                    y => y.ID == 1 || y.ID == 2 || y.ID == 3 || y.ID == 4 || y.ID == 5).ToList();

            paramGridControl.ItemsSource = Parameters;
        }

        public event EventHandler<CellParamEventArgs> ParameterSelected;

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            paramGridControl.View.PreviewMouseDoubleClick += PreviewMouseDoubleClick;
        }

        private new void PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParameterSelected(this,
                              new CellParamEventArgs {Parameter = paramGridControl.GetFocusedRow() as Y_NORM_PARAMETERS});
            Close();
        }
    }
}