using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using NormManagementMVVM.Model;
using System.Linq;
using NormManagementMVVM.ViewModel.Messages;

namespace NormManagementMVVM.ViewModel
{
    public class CellViewModel : ViewModelBase, ICloneable
    {
        public RowViewModel Row { get; private set; }
        public Controller Controller { get; private set; }
        private readonly Y_NORM_NORMATIVE_CELL _entity;

        #region Properties

        public bool IsEnabled
        {
            get
            {
                return Controller.All(cell => cell._entity.ID_COLUMN == cell.Row.Count);
            }
        }
        public Visibility RemoveButtonVisibility { get { return IsEnabled ? Visibility.Visible : Visibility.Hidden; } }
        public Visibility ValuesButtonVisibility { get { return IsEnabled && _entity.Y_NORM_PARAMETERS != null ? Visibility.Visible : Visibility.Hidden; } }
        public bool IsEnabledValues
        {
            get { return _entity.Y_NORM_PARAMETERS != null; }
        }
        public bool IsFilled
        {
            get { return _entity.ID_PARAM != 0 && _entity.PARAM_VALUE != null; }
        }
        public Brush CellColor
        {
            get
            {
                var controller = _entity.CONTROLLER;
                var rest = controller % 16;
                var brush = new SolidColorBrush { Color = Brushes.White.Color, Opacity = 1 };
                switch (rest)
                {
                    case 0:
                        {
                            brush.Color = Brushes.Black.Color;
                            break;
                        }
                    case 1:
                        {
                            brush.Color = Brushes.Green.Color;
                            break;
                        }
                    case 2:
                        {
                            brush.Color = Brushes.Yellow.Color;
                            break;
                        }
                    case 3:
                        {
                            brush.Color = Brushes.Brown.Color;
                            break;
                        }
                    case 4:
                        {
                            brush.Color = Brushes.LightSeaGreen.Color;
                            break;
                        }
                    case 5:
                        {
                            brush.Color = Brushes.Orange.Color;
                            break;
                        }
                    case 6:
                        {
                            brush.Color = Brushes.Red.Color;
                            break;
                        }
                    case 7:
                        {
                            brush.Color = Brushes.Pink.Color;
                            break;
                        }
                    case 8:
                        {
                            brush.Color = Brushes.Violet.Color;
                            break;
                        }
                    case 9:
                        {
                            brush.Color = Brushes.LightGreen.Color;
                            break;
                        }
                    case 10:
                        {
                            brush.Color = Brushes.Blue.Color;
                            break;
                        }
                    case 11:
                        {
                            brush.Color = Brushes.LightBlue.Color;
                            break;
                        }
                    case 12:
                        {
                            brush.Color = Brushes.Tomato.Color;
                            break;
                        }
                    case 13:
                        {
                            brush.Color = Brushes.YellowGreen.Color;
                            break;
                        }
                    case 14:
                        {
                            brush.Color = Brushes.Aquamarine.Color;
                            break;
                        }
                    case 15:
                        {
                            brush.Color = Brushes.Cornsilk.Color;
                            break;
                        }
                }
                brush.Opacity = 0.5;
                return brush;
            }

        }
        public long RowSeq { get { return _entity.Y_NORM_NORMATIVE_ROW.SEQ_NUM ?? 0; } }
        public string ParamName { get { return _entity.Y_NORM_PARAMETERS == null ? "..." : _entity.Y_NORM_PARAMETERS.DESC_RU; } }
        public string ParamValues
        {
            get
            {
                return _entity.ParamValueDesc == "null" ? "..." : _entity.ParamValueDesc;
            }
        }
        private Y_NORM_PARAMETERS Param
        {
            set
            {
                _entity.Y_NORM_PARAMETERS = value;
                _entity.PARAM_VALUE = string.Empty;
                RaisePropertyChanged("ParamName");
                RaisePropertyChanged("ParamValues");
                RaisePropertyChanged("IsEnabledValues");
                RaisePropertyChanged("ValuesButtonVisibility");
            }
        }
        private ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result> ParamValuesCollection
        {
            get
            {
                return GenericRepository.GetValues((int)_entity.ID_PARAM, _entity.PARAM_VALUE);
            }
        }

        #endregion

        #region Commands

        public RelayCommand CommandEditParameter { get; private set; }
        public RelayCommand CommandEditParameterValues { get; private set; }
        public RelayCommand CommandRemoveController { get; private set; }

        #endregion

        public CellViewModel(Y_NORM_NORMATIVE_CELL cell, RowViewModel rowViewModel)
        {
            _entity = cell;
            Row = rowViewModel;

            Messenger.Default.Register<GenericMessage<MessageArgsParameterCallback>>(this, MessageEditParameterCallbackHandler);
            Messenger.Default.Register<GenericMessage<MessageArgsParameterValuesCallback>>(this, MessageEditParameterValuesCallbackHandler);
            CommandEditParameter = new RelayCommand(CommandEditParameterRelease);
            CommandEditParameterValues = new RelayCommand(CommandEditParameterValuesRelease);
            CommandRemoveController = new RelayCommand(CommandRemoveControllerRelease);

            if (Controller == null)
            {
                if (_entity.CONTROLLER == null)
                {
                    _entity.CONTROLLER = HeadViewModel.Controllers.NewKey();
                    _entity.Y_NORM_NORMATIVE_CONTROLLER = new Y_NORM_NORMATIVE_CONTROLLER { CONTROLLER = (int)_entity.CONTROLLER };
                }
                _entity.CONTROLLER = _entity.CONTROLLER ?? HeadViewModel.Controllers.NewKey();
                if (!HeadViewModel.Controllers.ContainsKey(_entity.CONTROLLER))
                {
                    Controller = new Controller();
                    HeadViewModel.Controllers.Add(_entity.CONTROLLER, Controller);
                }
                else
                {
                    Controller = HeadViewModel.Controllers[_entity.CONTROLLER];
                }
            }
            Controller.Add(this);
        }

        private string GetClause()
        {
            var clause = "";
            if (_entity.Y_NORM_NORMATIVE_ROW.Y_NORM_NORMATIVE_CELL.Count != 1)
            {
                clause = _entity.Y_NORM_PARAMETERS.PARAM_TYPE == "STORE"
                             ? CreateClause(_entity, y => y.Y_NORM_PARAMETERS.PARAM_TYPE == "STORE")
                             : CreateClause(_entity,
                                            y =>
                                            y.ID_PARAM == 1 || y.ID_PARAM == 2 || y.ID_PARAM == 3 || y.ID_PARAM == 4 ||
                                            y.ID_PARAM == 5);
            }
            return clause;
        }
        private static string CreateClause(Y_NORM_NORMATIVE_CELL currentCell, Func<Y_NORM_NORMATIVE_CELL, bool> predicate)
        {
            string clause = "";
            foreach (
                Y_NORM_NORMATIVE_CELL cell in
                    currentCell.Y_NORM_NORMATIVE_ROW.Y_NORM_NORMATIVE_CELL.Where(predicate))
            {
                if (cell == currentCell) continue;
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

        #region Message Handlers

        private void MessageEditParameterCallbackHandler(GenericMessage<MessageArgsParameterCallback> message)
        {
            if (message.Target is CellViewModel == false) return;
            var target = message.Target as CellViewModel;
            if (target == null) return;
            if (target != this) return;

            Param = message.Content.Parameter;
        }
        private void MessageEditParameterValuesCallbackHandler(GenericMessage<MessageArgsParameterValuesCallback> message)
        {
            if (message.Target is CellViewModel == false) return;
            var target = message.Target as CellViewModel;
            if (target == null) return;
            if (target != this) return;

            Controller.InsertValues(message.Content.GroupsRight);
        }

        #endregion

        #region Commands Realization

        private void CommandEditParameterRelease()
        {
            var list = _entity.Y_NORM_NORMATIVE_ROW.Y_NORM_NORMATIVE_CELL.Select(cell => cell.Y_NORM_PARAMETERS).ToList();

            var message = new GenericMessage<MessageArgsParameter>(this, null,
                                              new MessageArgsParameter
                                              {
                                                  Parameters = list
                                              });

            Messenger.Default.Send(message);
            Messenger.Default.Send(new NotificationMessage("OpenEditParameters"));
        }
        private void CommandEditParameterValuesRelease()
        {
            if (_entity.ID_PARAM == 0) return;
            var values = new ObservableCollection<ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result>>();
            foreach (var cell in Controller)
            {
                if (cell.ParamValuesCollection.Count > 0)
                    values.Add(cell.ParamValuesCollection);
            }

            var message = new GenericMessage<MessageArgsParameterValues>(this, null,
                                              new MessageArgsParameterValues
                                              {
                                                  Id = (int)_entity.ID,
                                                  IdParam = (int)_entity.ID_PARAM,
                                                  Clause = GetClause(),
                                                  Values = values
                                              });

            Messenger.Default.Send(message);

            Messenger.Default.Send(new NotificationMessage("OpenEditParametersValues"));
        }
        private void CommandRemoveControllerRelease()
        {
            Controller.Clear();
        }

        #endregion

        public void Remove()
        {
            GenericRepository.Delete(_entity);
            Controller.Remove(this);
            Row.Remove(this);
        }
        public void RefreshState()
        {
            RaisePropertyChanged("IsEnabled");
            RaisePropertyChanged("RemoveButtonVisibility");
            RaisePropertyChanged("ValuesButtonVisibility");
        }
        public void Fill(ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result> group)
        {
            _entity.PARAM_VALUE = string.Join(",", group.Select(param => param.VALUE));
            RaisePropertyChanged("ParamValues");
        }

        #region Implementation of IClonable

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public object Clone(RowViewModel row)
        {
            var cellEntity = new Y_NORM_NORMATIVE_CELL
            {
                ID = _entity.ID,
                ID_ROW = _entity.ID_ROW,
                ID_COLUMN = _entity.ID_COLUMN,
                ID_PARAM = _entity.ID_PARAM,
                PARAM_VALUE = _entity.PARAM_VALUE,
                CONTROLLER = _entity.CONTROLLER,
                Y_NORM_NORMATIVE_CONTROLLER = _entity.Y_NORM_NORMATIVE_CONTROLLER,
                Y_NORM_NORMATIVE_ROW = row.Entity,
                Y_NORM_PARAMETERS = _entity.Y_NORM_PARAMETERS
            };
            row.Entity.Y_NORM_NORMATIVE_CELL.Add(cellEntity);
            var cell = new CellViewModel(cellEntity, row);
            return cell;
        }

        #endregion
    }
}