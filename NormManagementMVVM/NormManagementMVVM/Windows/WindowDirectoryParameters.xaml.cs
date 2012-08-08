using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using DevExpress.Xpf.Grid;
using NormManagementMVVM.Model;

namespace NormManagementMVVM.Windows
{
    /// <summary>
    /// Логика взаимодействия для WindowDirectoryParameters.xaml
    /// </summary>
    public partial class WindowDirectoryParameters
    {
        #region Delegates

        //public delegate void DirectoryParamEventHandler(object sender, DirectoryParamEventArgs e);

        #endregion

        private readonly ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result> _collection;

        public WindowDirectoryParameters(int idParam, bool multiSelect)
        {
            Id = idParam;
            IsMultiSelect = multiSelect;
            InitializeComponent();
            _collection = GenericRepository.GetParamValues(idParam);
            paramGridControl.ItemsSource = _collection;
            ((TableView)paramGridControl.View).ShowGroupPanel = false;
            
            ((TableView) paramGridControl.View).ShowAutoFilterRow = true;
            if (IsMultiSelect)
            {
                ((TableView) paramGridControl.View).MultiSelectMode = TableViewSelectMode.Row;
            }
        }

        private int Id { get; set; }
        private bool IsMultiSelect { get; set; }
        public event EventHandler<DirectoryParamEventArgs> DirectoryParamSelected;

        private void ParamGridControlMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (IsMultiSelect)
            {
                string values = string.Join(",",
                                            ((TableView)paramGridControl.View).SelectedRows.Select(
                                                y => ((Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result) y).VALUE));
                string names = string.Join(",",
                                           ((TableView)paramGridControl.View).SelectedRows.Select(
                                               y => ((Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result) y).NAME));
                DirectoryParamSelected(this,
                                       new DirectoryParamEventArgs
                                           {
                                               Id = Id,
                                               ParameterValues =
                                                   new Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result
                                                       {NAME = names, VALUE = values}
                                           });
            }
            else
            {
                var row = paramGridControl.GetFocusedRow() as Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result;
                DirectoryParamSelected(this, new DirectoryParamEventArgs {Id = Id, ParameterValues = row});
            }
            Close();
        }
    }
}