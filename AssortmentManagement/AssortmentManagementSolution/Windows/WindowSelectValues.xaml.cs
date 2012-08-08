using System;
using System.Linq;
using System.Windows;
using AssortmentManagement.Model;
using DevExpress.Xpf.Editors;

namespace AssortmentManagement.Windows
{

    public partial class WindowSelectValues
    {
        public delegate void ValuesEventHandler(object sender, ValuesEventArgs e);
        public event ValuesEventHandler ValuesSelected;

        public WindowSelectValues()
        {
            InitializeComponent();

            buttonApply.Click += ButtonApplyClick;
            textBoxValues.EditValueChanged += TextBoxValuesEditValueChanged;
        }

        private void TextBoxValuesEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            buttonApply.IsEnabled = textBoxValues.Text.Length > 0;
        }

        private void ButtonApplyClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var lines = textBoxValues.Text.Split(new[] { '\r', '\n', ',', ';', ' ' },
                                                     StringSplitOptions.RemoveEmptyEntries);
                ValuesSelected(this, new ValuesEventArgs { Values = lines.Distinct().ToList(), Result = true });
            }
            catch (Exception ex)
            {
                ValuesSelected(this, new ValuesEventArgs { Result = false, Error = ex.Message });
            }
            Close();
        }
    }
}
