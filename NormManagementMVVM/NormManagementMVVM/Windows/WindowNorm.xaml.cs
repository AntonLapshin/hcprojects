using System.ComponentModel;
using System.Windows;
using NormManagementMVVM.Model;

namespace NormManagementMVVM.Windows
{
    /// <summary>
    /// Логика взаимодействия для WindowNorm.xaml
    /// </summary>
    public partial class WindowNorm : ISaved
    {
        public WindowNorm()
        {
            InitializeComponent();
            normativeControl.ExceptionEvent += normativeControl_ExceptionEvent;
        }

        private bool isSaved { get; set; }

        #region ISaved Members

        public bool IsSaved { get; set; }

        #endregion

        private void normativeControl_ExceptionEvent(object sender, ExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            isSaved = false;
        }


        private void WindowClosing(object sender, CancelEventArgs e)
        {
            if (normativeControl.isChanged())
            {
                isSaved = true;
                MessageBoxResult result = MessageBox.Show("Желаете ли вы сохранить текущие изменения?", "Сохранимся?",
                                                          MessageBoxButton.YesNoCancel,
                                                          MessageBoxImage.Information)
                    ;
                if (!normativeControl.Saved(result))
                {
                    e.Cancel = true;
                }
                if (!isSaved)
                {
                    e.Cancel = true;
                }
            }
            IsSaved = normativeControl.IsSaved;
        }
    }
}