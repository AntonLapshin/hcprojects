using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using NormManagementMVVM.Model;
using NormManagementMVVM.ViewModel.Messages;

namespace NormManagementMVVM.ViewModel
{
    public class ParametersViewModel : ViewModelBase
    {
        private CellViewModel _cellViewModel;
        public List<Y_NORM_PARAMETERS> Parameters { get; private set; }
        public RelayCommand<Y_NORM_PARAMETERS> CommandSelectParameter { get; set; }

        public ParametersViewModel()
        {
            Messenger.Default.Register<GenericMessage<MessageArgsParameter>>(this, MessageParameterHandler);
            CommandSelectParameter = new RelayCommand<Y_NORM_PARAMETERS>(CommandSelectParameterRelease);
        }

        private void MessageParameterHandler(GenericMessage<MessageArgsParameter> message)
        {
            _cellViewModel = message.Sender as CellViewModel;
            Parameters = GenericRepository.GetAllList<Y_NORM_PARAMETERS>();
            foreach (var param in message.Content.Parameters)
            {
                Parameters.Remove(param);
            }
        }

        private void CommandSelectParameterRelease(Y_NORM_PARAMETERS param)
        {
            var message = new GenericMessage<MessageArgsParameterCallback>(this, _cellViewModel,
                                                                                 new MessageArgsParameterCallback { Parameter = param });
            Messenger.Default.Send(message);
            Messenger.Default.Send(new NotificationMessage("CloseEditParameter"));
        }
    }
}