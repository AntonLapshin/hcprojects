using System.Windows;
using GalaSoft.MvvmLight.Messaging;

namespace NormManagementMVVM.View
{
    /// <summary>
    /// Description for EditParameterView.
    /// </summary>
    public partial class EditParameterView : Window
    {
        /// <summary>
        /// Initializes a new instance of the EditParametersView class.
        /// </summary>
        public EditParameterView()
        {
            InitializeComponent();
            Messenger.Default.Register<NotificationMessage>(this, MessageNotificationHandler);
        }

        private void MessageNotificationHandler(NotificationMessage msg)
        {
            if (msg.Notification != "CloseEditParameter") return;
            Close();
        }
    }
}