using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Messaging;
using NormManagementMVVM.Model;

namespace NormManagementMVVM.ViewModel.Messages
{
    public class MessageParameterReturn : GenericMessage<MessageParameterReturnArgs>
    {
        private MessageParameterReturn(MessageParameterReturnArgs content) : base(content)
        {
            Args = content;
        }

        public MessageParameterReturnArgs Args { get; set; }

        public static MessageParameterReturn CreateMessage(CellViewModel viewModel, Y_NORM_PARAMETERS param)
        {
            return new MessageParameterReturn(MessageParameterReturnArgs.CreateMessageArgs(viewModel, param));
        }
    }

    public class MessageParameterReturnArgs
    {
        public static MessageParameterReturnArgs CreateMessageArgs(CellViewModel cellViewModel, Y_NORM_PARAMETERS param)
        {
            return new MessageParameterReturnArgs(cellViewModel, param);
        }

        private MessageParameterReturnArgs(CellViewModel cellViewModel, Y_NORM_PARAMETERS param)
        {
            CellViewModel = cellViewModel;
            Param = param;
        }

        public CellViewModel CellViewModel { get; set; }
        public Y_NORM_PARAMETERS Param { get; set; }
    }
}
