using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using NormManagementMVVM.Model;

namespace NormManagementMVVM.ViewModel.Messages
{
    public class MessageCellRemove : GenericMessage<MessageCellViewModelArgs>
    {
        private MessageCellRemove(MessageCellViewModelArgs content)
            : base(content)
        {
            Args = content;
        }

        public MessageCellViewModelArgs Args { get; set; }
        public static MessageCellRemove CreateMessage(CellViewModel viewModel)
        {
            return new MessageCellRemove(new MessageCellViewModelArgs { CellViewModelSource = viewModel });
        }
    }

    public class MessageCellViewModelArgs
    {
        public CellViewModel CellViewModelSource { get; set; }
    }
}
