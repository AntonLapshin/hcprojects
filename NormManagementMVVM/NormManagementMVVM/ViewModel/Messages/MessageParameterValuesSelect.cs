using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Messaging;
using NormManagementMVVM.Model;

namespace NormManagementMVVM.ViewModel.Messages
{
    public class MessageParameterValuesSelect : GenericMessage<MessageParameterValuesSelectArgs>
    {
        private MessageParameterValuesSelect(MessageParameterValuesSelectArgs content)
            : base(content)
        {
            Args = content;
        }

        public MessageParameterValuesSelectArgs Args { get; set; }

        public static MessageParameterValuesSelect CreateMessageNew(CellViewModel viewModelCallback, int id, string clause, int profile)
        {
            return
                new MessageParameterValuesSelect(
                    MessageParameterValuesSelectArgs.CreateParameterValuesCreateMessageArgsNew(viewModelCallback, id, clause,
                                                                                                       profile));
        }

        public static MessageParameterValuesSelect CreateMessageEdit(CellViewModel viewModelCallback, int id, string clause, int profile,IEnumerable<ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result>> values)
        {
            return
                new MessageParameterValuesSelect(
                    MessageParameterValuesSelectArgs.CreateParameterValuesCreateMessageArgsEdit(viewModelCallback,id, clause,
                                                                                                       profile, values));
        }
    }
    public class MessageParameterValuesSelectArgs
    {
        public static MessageParameterValuesSelectArgs CreateParameterValuesCreateMessageArgsEdit(CellViewModel viewModelCallback, int id, string clause, int profile, IEnumerable<ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result>> values)
        {
            return new MessageParameterValuesSelectArgs(viewModelCallback, id, clause, profile, values);
        }

        public static MessageParameterValuesSelectArgs CreateParameterValuesCreateMessageArgsNew(CellViewModel viewModelCallback, int id, string clause, int profile)
        {
            return new MessageParameterValuesSelectArgs(viewModelCallback, id, clause, profile);
        }

        private MessageParameterValuesSelectArgs(CellViewModel viewModelCallback, int id, string clause, int profile)
        {
            ViewModelCallback = viewModelCallback;
            Id = id;
            Clause = clause;
            Profile = profile;
        }

        private MessageParameterValuesSelectArgs(CellViewModel viewModelCallback, int id, string clause, int profile, IEnumerable<ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result>> values)
        {
            ViewModelCallback = viewModelCallback;
            Id = id;
            Clause = clause;
            Profile = profile;
            Values = values;
        }
        public CellViewModel ViewModelCallback { get; set; }
        public int Id { get; set; }
        public string Clause { get; set; }
        public int Profile { get; set; }
        public IEnumerable<ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result>> Values { get; set; }
    }
}
