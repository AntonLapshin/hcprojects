using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using NormManagementMVVM.Model;

namespace NormManagementMVVM.ViewModel.Messages
{
    public class MessageParameterSelect : GenericMessage<MessageParameterSelectArgs>
    {
        private MessageParameterSelect(MessageParameterSelectArgs content)
            : base(content)
        {
            Args = content;
        }

        public MessageParameterSelectArgs Args { get; set; }
        public static MessageParameterSelect CreateMessage(CellViewModel viewModel, List<Y_NORM_PARAMETERS> parameters)
        {
            return new MessageParameterSelect(MessageParameterSelectArgs.CreateMessageArgs(viewModel,parameters));
        }
    }

    public class MessageParameterSelectArgs
    {
        public static MessageParameterSelectArgs CreateMessageArgs(CellViewModel viewModel, List<Y_NORM_PARAMETERS> parameters)
        {
            return new MessageParameterSelectArgs(viewModel, parameters);
        }

        public CellViewModel ViewModelCallback { get; set; }
        public List<Y_NORM_PARAMETERS> ListUsedParameters { get; set; }

        private MessageParameterSelectArgs(CellViewModel viewModel, List<Y_NORM_PARAMETERS> parameters)
        {
            ViewModelCallback = viewModel;
            ListUsedParameters = parameters;
        }
    }
}
