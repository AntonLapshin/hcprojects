using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NormManagementMVVM.Model;
using NormManagementMVVM.Windows;
using SharedComponents.UI;

namespace NormManagementMVVM.Controls
{
    /// <summary>
    /// Логика взаимодействия для CellControl.xaml
    /// </summary>
    public partial class CellControl
    {
//      
        private ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result> _parameterValues;
        private ToolTip _ttParam;
        private ToolTip _ttValue;

        public CellControl()
        {
            InitializeComponent();
            DataContextChanged += CellControlDataContextChanged;
        }

        public ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result> Values
        {
            get { return _parameterValues; }
            set
            {
                _parameterValues = value;
                ParamValuesChanged(_parameterValues);
            }
        }

        public GroupCellController CellController { get; set; }

        private void SetCellController(Y_NORM_NORMATIVE_CELL cellDataContext)
        {
            if (cellDataContext.CONTROLLER == null)
            {
                cellDataContext.CONTROLLER = 0;
            }

            if (Controllers.CellControllers.ContainsKey(cellDataContext.CONTROLLER))
            {
                CellController = Controllers.CellControllers[cellDataContext.CONTROLLER];
            }
            else
            {
                int? newController = cellDataContext.CONTROLLER == 0 ? Controllers.NewKey() : cellDataContext.CONTROLLER;
                CellController = new GroupCellController();
                if (cellDataContext.CONTROLLER == 0) cellDataContext.CONTROLLER = newController;
                Controllers.CellControllers.Add(newController, CellController);
            }
            CellController.CellAdd(this);
        }

        private void SetVisibleParams(Y_NORM_NORMATIVE_CELL cellDataContext)
        {
            InitializeBtnTooltips(cellDataContext);

            btnParam.Content = cellDataContext.ID_PARAM != 0 ? cellDataContext.Y_NORM_PARAMETERS.DESC_RU : "...";

            InitializeParamValues(cellDataContext);
        }

        private void InitializeParamValues(Y_NORM_NORMATIVE_CELL cellDataContext)
        {
            _parameterValues = new ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result>();
            if (cellDataContext.ID_PARAM == 0) return;
            _parameterValues = GenericRepository.GetValues((Int32) cellDataContext.ID_PARAM, cellDataContext.PARAM_VALUE);

            lblParamValues.Content = string.Join(",", _parameterValues.Select(y => y.NAME));
            _ttValue.Content = string.Join(",", _parameterValues.Select(y => y.NAME));
        }

        private void InitializeBtnTooltips(Y_NORM_NORMATIVE_CELL cellDataContext)
        {
            if (cellDataContext.ID_PARAM != 0)
                _ttParam = new ToolTip {Content = cellDataContext.Y_NORM_PARAMETERS.DESC_RU};

            _ttValue = new ToolTip();
            lblParamValues.ToolTip = _ttValue;

            btnParam.ToolTip = _ttParam;
        }

        private void CellControlDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var cellDataContext = e.NewValue as Y_NORM_NORMATIVE_CELL;
            SetCellController(cellDataContext);
            SetVisibleParams(cellDataContext);
        }

        private void WinParamParameterSelected(object sender, CellParamEventArgs e)
        {
            btnParam.Content = e.Parameter.DESC_RU;
            ((Y_NORM_NORMATIVE_CELL) DataContext).Y_NORM_PARAMETERS = e.Parameter;
            _ttParam = new ToolTip {Content = ((Y_NORM_NORMATIVE_CELL) DataContext).Y_NORM_PARAMETERS.DESC_RU};
            btnParam.ToolTip = _ttParam;
            ClearDependancyRows();
            Values.Clear();
            lblParamValues.Content = "";
        }

        private void BtnParamValuesClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var cellDataContext = DataContext as Y_NORM_NORMATIVE_CELL;
                if (((Y_NORM_NORMATIVE_CELL) DataContext).ID_PARAM == 0) return;
                string clause = GetClause();
                WindowEditParameterValues winParamValues = CellController.Group.Count == 1
                                                               ? new WindowEditParameterValues(
                                                                     (int) cellDataContext.ID_PARAM, clause,
                                                                     (int)
                                                                     cellDataContext.Y_NORM_NORMATIVE_ROW.
                                                                         Y_NORM_NORMATIVE_HEAD.ID)
                                                               : new WindowEditParameterValues(
                                                                     CellController.Group.Select(cell => cell.Values.ToObservableCollection()).
                                                                         ToList());

                winParamValues.ParameterValuesSelected += WinParamValuesParameterValuesSelected;
                winParamValues.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetClause()
        {
            var cellDataContext = DataContext as Y_NORM_NORMATIVE_CELL;
            string clause = "";
            if (cellDataContext.Y_NORM_NORMATIVE_ROW.Y_NORM_NORMATIVE_CELL.Count != 1)
            {
                clause = cellDataContext.Y_NORM_PARAMETERS.PARAM_TYPE == "STORE"
                             ? CreateClause(cellDataContext, y => y.Y_NORM_PARAMETERS.PARAM_TYPE == "STORE")
                             : CreateClause(cellDataContext,
                                            y =>
                                            y.ID_PARAM == 1 || y.ID_PARAM == 2 || y.ID_PARAM == 3 || y.ID_PARAM == 4 ||
                                            y.ID_PARAM == 5);
            }
            return clause;
        }

        private string CreateClause(Y_NORM_NORMATIVE_CELL cellDataContext, Func<Y_NORM_NORMATIVE_CELL, bool> predicate)
        {
            string clause = "";
            foreach (
                Y_NORM_NORMATIVE_CELL cell in
                    cellDataContext.Y_NORM_NORMATIVE_ROW.Y_NORM_NORMATIVE_CELL.Where(predicate))
            {
                if (cell == cellDataContext) continue;
                if (clause == "")
                {
                    clause = cell.Y_NORM_PARAMETERS.DESCRIPTION + " in (" + cell.PARAM_VALUE + ")";
                }
                else
                {
                    clause += " and " + cell.Y_NORM_PARAMETERS.DESCRIPTION + " in (" + cell.PARAM_VALUE + ")";
                }
            }
            return clause;
        }

        private void WinParamValuesParameterValuesSelected(object sender, ParamValuesEventArgs e)
        {
            if (CellController.Group.Count != 1)
            {
                ClearDependancyRows();
            }
            ChangeMassSeq((int) ((Y_NORM_NORMATIVE_CELL) DataContext).Y_NORM_NORMATIVE_ROW.SEQ_NUM,
                          e.ParamValues.Count - 1, MassChangeValuesType.Add);
            foreach (var values in e.ParamValues)
            {
                if (e.ParamValues.IndexOf(values) == 0)
                {
                    Values = values;
                }
                else
                {
                    CellController.RowCopy(this, values, e.ParamValues.IndexOf(values));
                }
            }
        }

        private void BtnParamDefaultButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var winParam = new WindowEditParameters((Y_NORM_NORMATIVE_CELL) DataContext);
                winParam.ParameterSelected += WinParamParameterSelected;
                winParam.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ParamValuesChanged(
            ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result> parameterValues)
        {
            ((Y_NORM_NORMATIVE_CELL) DataContext).PARAM_VALUE = string.Join(",",
                                                                            parameterValues.Select(y => y.VALUE));

            lblParamValues.Content = string.Join(",", parameterValues.Select(y => y.NAME));
            _ttValue.Content = string.Join(",", parameterValues.Select(y => y.NAME));
        }

        private void ClearDependancyRows()
        {
            var tempCellList = new List<CellControl>();
            long? minSeq = ((Y_NORM_NORMATIVE_CELL) DataContext).Y_NORM_NORMATIVE_ROW.SEQ_NUM;
            foreach (CellControl cell in CellController.Group)
            {
                if (cell == this) continue;
                var rowControl = UIHelper.FindVisualParent<RowControl>(cell);
                foreach (UIElement cellControl in rowControl.rowPanel.Children)
                {
                    if (cellControl.GetType() != typeof (CellControl) ||
                        rowControl.rowPanel.Children.IndexOf(cellControl) == rowControl.rowPanel.Children.Count - 2)
                        continue;

                    ((CellControl) cellControl).CellController.Group.Remove((CellControl) cellControl);
                }
                var normControl = UIHelper.FindVisualParent<NormativeControl>(rowControl);
                var row = ((Y_NORM_NORMATIVE_ROW) rowControl.DataContext);
                minSeq = row.SEQ_NUM < minSeq ? row.SEQ_NUM : minSeq;
                GenericRepository.Delete(row);
                normControl.normPanel.Children.Remove(rowControl);
                tempCellList.Add(cell);
            }
            ((Y_NORM_NORMATIVE_CELL) DataContext).Y_NORM_NORMATIVE_ROW.SEQ_NUM = minSeq;

            ChangeMassSeq((int) ((Y_NORM_NORMATIVE_CELL) DataContext).Y_NORM_NORMATIVE_ROW.SEQ_NUM, tempCellList.Count,
                          MassChangeValuesType.Delete);

            foreach (CellControl cellControl in tempCellList)
            {
                CellController.Group.Remove(cellControl);
            }
        }

        private void ChangeMassSeq(int index, int count, MassChangeValuesType type)
        {
            foreach (
                Y_NORM_NORMATIVE_ROW row in
                    ((Y_NORM_NORMATIVE_CELL) DataContext).Y_NORM_NORMATIVE_ROW.Y_NORM_NORMATIVE_HEAD.
                        Y_NORM_NORMATIVE_ROW.Where(y => y.SEQ_NUM > index))
            {
                row.SeqNumChange(count, type);
            }
        }

        private void LblDeleteMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DeletedRowValuesChange();

                ClearDependancyRows();

                RemoveVisualRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveVisualRow()
        {
            GenericRepository.Delete((Y_NORM_NORMATIVE_CELL) DataContext);
            var rowControl = UIHelper.FindVisualParent<RowControl>(this);

            Controllers.Remove(CellController);

            rowControl.rowPanel.Children.Remove(this);
            if (rowControl.rowPanel.Children[rowControl.rowPanel.Children.Count - 2].GetType() ==
                typeof (CellControl))
            {
                ((CellControl) rowControl.rowPanel.Children[rowControl.rowPanel.Children.Count - 2]).CellController.
                    CheckController();
            }
        }

        private void DeletedRowValuesChange()
        {
            ((Y_NORM_NORMATIVE_CELL) DataContext).Y_NORM_NORMATIVE_ROW.SKU =
                CellController.Group.Aggregate<CellControl, long>(0,
                                                                  (current, control) =>
                                                                  current +
                                                                  ((Y_NORM_NORMATIVE_CELL) control.DataContext).
                                                                      Y_NORM_NORMATIVE_ROW.SKU);
            ((Y_NORM_NORMATIVE_CELL) DataContext).Y_NORM_NORMATIVE_ROW.DELTA =
                CellController.Group.Aggregate<CellControl, decimal?>(0,
                                                                      (current, control) =>
                                                                      current +
                                                                      ((Y_NORM_NORMATIVE_CELL) control.DataContext).
                                                                          Y_NORM_NORMATIVE_ROW.DELTA);
            ((Y_NORM_NORMATIVE_CELL) DataContext).Y_NORM_NORMATIVE_ROW.MAX_COLUMN =
                (short) (((Y_NORM_NORMATIVE_CELL) DataContext).Y_NORM_NORMATIVE_ROW.Y_NORM_NORMATIVE_CELL.Count - 1);
        }
    }
}