using System;
using System.Windows;
using System.Windows.Input;
using AssortmentManagement.Model;

namespace AssortmentManagement.Windows
{
    public partial class WindowDescription
    {
        public event EventHandler DescriptionChanged;
        private readonly Document _doc;

        public WindowDescription(Document document)
        {
            InitializeComponent();

            KeyDown += WindowDescriptionKeyDown;

            _doc = document;
            textBox1.Text = _doc.Description;
            textBox1.Focus();
            textBox1.SelectAll();
        }

        private void WindowDescriptionKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ApplyMethod();
            }
        }

        private void Button1Click(object sender, RoutedEventArgs e)
        {
            ApplyMethod();
        }

        private void ApplyMethod()
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("Комментарий не должен быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (textBox1.Text.Length > 255)
            {
                MessageBox.Show("Комментарий не должен содержать более 256 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                try
                {
                    _doc.Description = textBox1.Text;
                }
                catch (AssortmentException e)
                {
                    MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                DescriptionChanged(this, new EventArgs());
                Close();
            }
        }
    }
}
