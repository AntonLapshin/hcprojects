using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using DevExpress.Xpf.Editors;
using GalaSoft.MvvmLight.Messaging;
using NormManagementMVVM.Model;
using NormManagementMVVM.ViewModel;

namespace NormManagementMVVM.View
{
    /// <summary>
    /// Description for EditParameterValuesView.
    /// </summary>
    public partial class EditParameterValuesView : Window
    {
        /// <summary>
        /// Initializes a new instance of the EditParameterValuesView class.
        /// </summary>
        public EditParameterValuesView()
        {
            InitializeComponent();
            Messenger.Default.Register<NotificationMessage>(this, MessageNotificationHandler);
        }

        private void MessageNotificationHandler(NotificationMessage msg)
        {
            if (msg.Notification != "CloseEditParameterValues") return;
            Close();
        }

        private void listBoxAllParams_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var listBox = sender as ListBoxEdit;
            if (listBox == null) return;
            
            var viewModel = listBox.Tag as ParameterValuesViewModel;
            if (viewModel == null) return;

            viewModel.ValuesLeftSelected.Clear();

            foreach(Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result item in listBox.SelectedItems)
            {
                viewModel.ValuesLeftSelected.Add(item);
            }
        }

        private void ListBoxEdit_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var listBox = sender as ListBoxEdit;
            if (listBox == null) return;

            var viewModel = listBox.Tag as ParameterValuesViewModel;
            if (viewModel == null) return;

            viewModel.ValuesRightSelected.Clear();

            foreach (Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result item in listBox.SelectedItems)
            {
                viewModel.ValuesRightSelected.Add(item);
            }
        }
    }
}