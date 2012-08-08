using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Editors;
using NormManagementMVVM.Model;

namespace NormManagementMVVM.Windows
{
    /// <summary>
    /// Логика взаимодействия для WindowParameterValues.xaml
    /// </summary>
    public partial class WindowEditParameterValues
    {
        private readonly ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result> _parameters;

        public WindowEditParameterValues(int id, string clause, int profile)
        {
            InitializeComponent();

            _parameters = GenericRepository.GetParamValues(id, clause, profile);

            listBoxAllParams.ItemsSource = _parameters;
            ParametersSort(false);
        }

        public WindowEditParameterValues(
            IEnumerable<ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result>> values)
        {
            InitializeComponent();

            _parameters = new ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result>();

            listBoxAllParams.ItemsSource = _parameters;
            foreach (var value in values)
            {
                var listBoxSelected = new ListBoxEdit {SelectionMode = SelectionMode.Extended, ItemsSource = value};
                rowParams.Children.Add(listBoxSelected);
            }
            ParametersSort(false);
        }

        public event EventHandler<ParamValuesEventArgs> ParameterValuesSelected;
 
        private void BtnRightClick(object sender, RoutedEventArgs e)
        {
            if (_parameters.Count == 0 || listBoxAllParams.SelectedItems.Count == 0) return;

            var listBoxSelected = new ListBoxEdit {SelectionMode = SelectionMode.Extended};
            var collection = new ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result>();
            foreach (object selectedItem in listBoxAllParams.SelectedItems)
            {
                collection.Add((Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result) selectedItem);
            }
            listBoxSelected.ItemsSource = collection;
            rowParams.Children.Add(listBoxSelected);
            foreach (Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result o in collection)
            {
                _parameters.Remove(o);
            }
            listBoxAllParams.SelectedItems.Clear();
        }

        private void BtnLeftClick(object sender, RoutedEventArgs e)
        {
            var collection = new List<ListBoxEdit>();
            foreach (object listbox in rowParams.Children)
            {
                var list = ((ListBoxEdit) listbox).SelectedItems.Cast<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result>().ToList();
                foreach (Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result item in list)
                {
                    _parameters.Add(item);
                    ((ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result>)
                     ((ListBoxEdit) listbox).ItemsSource).Remove(item);
                }
                if (
                    ((ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result>)
                     ((ListBoxEdit) listbox).ItemsSource).Count == 0)
                {
                    collection.Add((ListBoxEdit) listbox);
                }
                listBoxAllParams.SelectedItems.Clear();
                ((ListBoxEdit) listbox).SelectedItems.Clear();
            }
            foreach (ListBoxEdit listBoxEdit in collection)
            {
                rowParams.Children.Remove(listBoxEdit);
            }

            TrackBarSort(Convert.ToInt32(trackBarEdit1.Content));
        }

        private void BtnGroupBackClick(object sender, RoutedEventArgs e)
        {
            var collection = rowParams.Children.Cast<ListBoxEdit>().Where(listbox => listbox.SelectedItems.Count != 0).ToList();
            foreach (var listBoxEdit in collection)
            {
                foreach (var item in listBoxEdit.ItemsSource)
                {
                    _parameters.Add((Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result) item);
                    listBoxAllParams.SelectedItems.Clear();
                }
                rowParams.Children.Remove(listBoxEdit);
            }
            TrackBarSort(Convert.ToInt32(trackBarEdit1.Content));
        }

        private void BtnAllClick(object sender, RoutedEventArgs e)
        {
            
            if (_parameters.Count == 0) return;
            var listBoxSelected = new ListBoxEdit {SelectionMode = SelectionMode.Extended};
            var collection = new ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result>();
            
            foreach (Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result selectedItem in _parameters)
            {
                collection.Add(selectedItem);
            }
            listBoxSelected.ItemsSource = collection;
            rowParams.Children.Add(listBoxSelected);
            foreach (Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result o in collection)
            {
                _parameters.Remove(o);
            }
        }

        private void BtnAllBackClick(object sender, RoutedEventArgs e)
        {
            foreach (object listBoxItems in rowParams.Children)
            {
                foreach (object item in ((ListBoxEdit) listBoxItems).ItemsSource)
                {
                    _parameters.Add((Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result) item);
                }
            }
            rowParams.Children.Clear();
        }

        private void BtnOkClick(object sender, RoutedEventArgs e)
        {
            if (_parameters.Count != 0)
            {
                MessageBox.Show("Вы выбрали не все параметры!", "Предупреждение", MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
            else
            {
                var paramEventArgs = new ParamValuesEventArgs
                                         {
                                             ParamValues =
                                                 new List
                                                 <ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result>>()
                                         };
                foreach (object listbox in rowParams.Children)
                {
                    paramEventArgs.ParamValues.Add(
                        (ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result>)
                        ((ListBoxEdit) listbox).ItemsSource);
                }

                ParameterValuesSelected(this, paramEventArgs);
                Close();
            }
        }

        private void ParametersSort(bool sort)
        {
            if (sort)
            {
                _parameters.SortDesc(y => y.NAME);
            }
            else
            {
                _parameters.Sort(y => y.NAME);
            }
        }


        private void TrackBarSort(int value)
        {
            if (value == 1)
            {
                ParametersSort(true);
            }
            if (value == 0)
            {
                ParametersSort(false);
            }
        }

        private void TrackBarEdit1EditValueChanging(object sender, EditValueChangingEventArgs e)
        {
            if (e.NewValue == null) return;

            TrackBarSort(Convert.ToInt32(e.NewValue));
        }
    }
}