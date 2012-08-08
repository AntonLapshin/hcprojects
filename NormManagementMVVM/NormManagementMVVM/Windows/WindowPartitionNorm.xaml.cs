using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NormManagementMVVM.Model;
using SharedComponents.Connection;

namespace NormManagementMVVM.Windows
{
    /// <summary>
    /// Логика взаимодействия для WindowPartitionNorm.xaml
    /// </summary>
    public partial class WindowPartitionNorm : Window
    {
        public WindowPartitionNorm()
        {
            InitializeComponent();
            User.Name = "RMSPRD";
            User.Password = "golive104";
            RMSConnection.Current = RMSConnection.RMSTSTN;
            GenericRepository.InitializeServer();
            cmbProfile.ItemsSource = GenericRepository.GetAll<Y_NORM_PROFILE_HEAD>().Where(y => y.Y_NORM_PROFILE_DETAIL.Count != 0).
                        OrderBy(y => y.ToString());
        }

        private void CmbProfileSelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            scrollViewer.DataContext = GenericRepository.FindOne<Y_NORM_NORMATIVE_HEAD>(
                        x => x.ID == ((Y_NORM_PROFILE_HEAD)cmbProfile.SelectedItem).ID) ??
                    new Y_NORM_NORMATIVE_HEAD
                    {
                        ID = ((Y_NORM_PROFILE_HEAD)cmbProfile.SelectedItem).ID,
                        Y_NORM_PROFILE_HEAD = (Y_NORM_PROFILE_HEAD)cmbProfile.SelectedItem,
                        CREATE_DATETIME = DateTime.Now,
                        LAST_UPDATE_ID = User.Name.ToUpper()
                    };
        }
    }
}
