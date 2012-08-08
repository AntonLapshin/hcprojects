using System.Windows;
using DevExpress.Xpf.Core;
using GalaSoft.MvvmLight.Messaging;

namespace NormManagementMVVM.View
{
    /// <summary>
    /// Description for NormView.
    /// </summary>
    public partial class NormView : Window
    {
        /// <summary>
        /// Initializes a new instance of the NormView class.
        /// </summary>
        public NormView()
        {
            Messenger.Default.Register<NotificationMessage>(this, MessageNotificationHandler);
            InitializeComponent();
            ThemeManager.SetThemeName(this, "Seven");
        }

        private static void MessageNotificationHandler(NotificationMessage msg)
        {
            switch (msg.Notification)
            {
                case "OpenEditParameters":
                    {
                        var win = new EditParameterView();
                        win.ShowDialog();
                        break;
                    }
                case "OpenEditParametersValues":
                    {
                        var win = new EditParameterValuesView();
                        win.ShowDialog();
                        break;
                    }
            }
        }
    }
}