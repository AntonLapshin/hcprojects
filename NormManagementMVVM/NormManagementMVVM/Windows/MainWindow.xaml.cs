using System.Windows;
using System.Windows.Input;

namespace NormManagement.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonChangeClick(object sender, RoutedEventArgs e)
        {
            Enter();
        }

        private void Enter()
        {
            //var winNorm = new WindowNorm(int.Parse(textBox1.Text));
            //winNorm.ShowDialog();
        }

        private void WindowKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Enter();
        }
    }
}
