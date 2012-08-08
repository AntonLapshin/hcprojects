using System.Windows;
using NormManagementMVVM.Model;
using SharedComponents.Connection;

namespace NormManagementMVVM.Windows
{
    /// <summary>
    /// Логика взаимодействия для WindowServerChoose.xaml
    /// </summary>
    public partial class WindowServerChoose : Window
    {
        public WindowServerChoose()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (tstRadioBtn.IsChecked == true)
            {
                //Login.Name = "RMSTST";
                //Login.Password = "rmstst";
                User.Name = "RMSPRD";
                User.Password = "golive104";
                RMSConnection.Current = RMSConnection.RMSTSTN;
            }
            else if (prodRadioBtn.IsChecked == true)
            {
                User.Name = "RMSPRD";
                User.Password = "golive104";
                RMSConnection.Current = RMSConnection.RMSP;
            }
            var win = new WindowMain();
            Hide();
            win.ShowDialog();
            Show();
        }
    }
}