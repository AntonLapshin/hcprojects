using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using NormManagementMVVM.Model;
using NormManagementMVVM.ViewModel.Messages;

namespace NormManagementMVVM.ViewModel
{
    public class ParameterValuesViewModel : ViewModelBase
    {
        private CellViewModel _cellViewModel;

        private ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result> _valuesLeft;
        private ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result> _valuesLeftSelected;
        private ObservableCollection<ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result>> _groupsRight;
        private ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result> _valuesRightSelected;

        private int _sortType;
        public int SortType
        {
            get { return _sortType; }
            set
            {
                _sortType = value;
                ParametersSort();
                RaisePropertyChanged("SortType");
            }
        }

        public string Title
        {
            get { return "Выбор значений параметра (" + _cellViewModel.ParamName + ")"; }
        }

        public ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result> ValuesLeft
        {
            get
            {
                return _valuesLeft;
            }
            set
            {
                _valuesLeft = value;
                RaisePropertyChanged("ValuesLeft");
            }
        }
        public ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result> ValuesLeftSelected
        {
            get
            {
                return _valuesLeftSelected;
            }
            set
            {
                _valuesLeftSelected = value;
                RaisePropertyChanged("ValuesLeftSelected");
            }
        }
        public ObservableCollection<ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result>> GroupsRight
        {
            get
            {
                return _groupsRight;
            }
            set
            {
                _groupsRight = value;
                RaisePropertyChanged("GroupsRight");
            }
        }
        public ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result> ValuesRightSelected
        {
            get
            {
                return _valuesRightSelected;
            }
            set
            {
                _valuesRightSelected = value;
                RaisePropertyChanged("ValuesRightSelected");
            }
        }

        public RelayCommand CommandMoveRight { get; set; }
        public RelayCommand CommandMoveLeft { get; set; }
        public RelayCommand CommandMoveLeftGroup { get; set; }
        public RelayCommand CommandMoveLeftAll { get; set; }
        public RelayCommand CommandMoveRightAll { get; set; }
        public RelayCommand CommandProcess { get; set; }

        public ParameterValuesViewModel()
        {
            Messenger.Default.Register<GenericMessage<MessageArgsParameterValues>>(this, MessageParameterValuesHandler);
            CommandMoveLeftAll = new RelayCommand(CommandMoveLeftAllRelease);
            CommandMoveLeft = new RelayCommand(CommandMoveLeftRelease);
            CommandMoveLeftGroup = new RelayCommand(CommandMoveLeftGroupRelease);
            CommandMoveRightAll = new RelayCommand(CommandMoveRightAllRelease);
            CommandMoveRight = new RelayCommand(CommandMoveRightRelease);
            CommandProcess = new RelayCommand(CommandProcessRelease);
        }

        private void MessageParameterValuesHandler(GenericMessage<MessageArgsParameterValues> message)
        {
            _valuesLeft = GenericRepository.GetParamValues(message.Content.IdParam, message.Content.Clause,
                                                              message.Content.Id);
            _groupsRight =
                new ObservableCollection<ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result>>();
            _valuesLeftSelected = new ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result>();
            _valuesRightSelected = new ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result>();

            SortType = 0;

            _cellViewModel = message.Sender as CellViewModel;
            foreach (var value in message.Content.Values)
            {
                if (value == null) continue;
                GroupsRight.Add(value);
                var temp = new ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result>();
                foreach (var parameters in value.SelectMany(param => ValuesLeft.Where(parameters => param.VALUE == parameters.VALUE)))
                {
                    temp.Add(parameters);
                }
                foreach (var par in temp)
                {
                    ValuesLeft.Remove(par);
                }
            }
            ParametersSort();
        }
        private void ParametersSort()
        {
            switch (SortType)
            {
                case 0:
                    ValuesLeft.SortDesc(y => y.NAME);
                    break;
                case 1:
                    ValuesLeft.Sort(y => y.NAME);
                    break;
            }
        }
        private ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result> GetGroupByValue(Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result value)
        {
            /*
                        foreach (var group in GroupsRight)
                        {
                            foreach (var val in group)
                            {
                                if (val == value) return group;
                            }
                        }
             */
            return GroupsRight.FirstOrDefault(group => group.Any(val => val == value));
        }

        private void CommandMoveLeftAllRelease()
        {
            foreach (var group in GroupsRight)
            {
                foreach (var value in group)
                {
                    ValuesLeft.Add(value);
                }
            }
            GroupsRight.Clear();
        }
        private void CommandMoveLeftRelease()
        {
            foreach (var value in ValuesRightSelected)
            {
                ValuesLeft.Add(value);

                var group = GetGroupByValue(value);
                group.Remove(value);
                if (group.Count == 0) GroupsRight.Remove(group);
            }
            ValuesRightSelected.Clear();
        }
        private void CommandMoveLeftGroupRelease()
        {
            var group = GetGroupByValue(ValuesRightSelected.FirstOrDefault());
            if (group == null) return;
            foreach (var value in group)
            {
                ValuesLeft.Add(value);
            }
            GroupsRight.Remove(group);
            ValuesRightSelected.Clear();
        }
        private void CommandMoveRightAllRelease()
        {
            if (ValuesLeft.Count == 0) return;
            var group = new ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result>();
            foreach (var item in ValuesLeft)
            {
                group.Add(item);
            }
            ValuesLeft.Clear();
            GroupsRight.Add(group);
        }
        private void CommandMoveRightRelease()
        {
            if (ValuesLeft.Count == 0 || ValuesLeftSelected.Count == 0) return;
            var group = new ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result>();
            foreach (var selectedItem in ValuesLeftSelected)
            {
                group.Add(selectedItem);
            }
            foreach (var item in ValuesLeftSelected)
            {
                ValuesLeft.Remove(item);
            }
            GroupsRight.Add(group);
            ValuesLeftSelected.Clear();
        }
        private void CommandProcessRelease()
        {
            var message = new GenericMessage<MessageArgsParameterValuesCallback>(this, _cellViewModel,
                                                                                 new MessageArgsParameterValuesCallback { GroupsRight = GroupsRight });

            Messenger.Default.Send(message);
            Messenger.Default.Send(new NotificationMessage("CloseEditParameterValues"));
        }
    }
}