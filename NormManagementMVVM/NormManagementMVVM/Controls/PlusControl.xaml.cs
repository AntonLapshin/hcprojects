using System.Windows;

namespace NormManagement.Controls
{
    /// <summary>
    /// Логика взаимодействия для PlusControl.xaml
    /// </summary>
    public partial class PlusControl
    {
        #region Delegates

        public delegate void PlusEventHandler(object sender, RoutedEventArgs e);

        #endregion

        public PlusControl()
        {
            InitializeComponent();
        }

        public event PlusEventHandler PlusClick;

        private void BtnPlusClick(object sender, RoutedEventArgs e)
        {
            PlusClick(this, e);
        }
    }
}