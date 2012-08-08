using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using AssortmentManagement.Model;
using DevExpress.Xpf.Editors;

namespace AssortmentManagement
{

    public partial class WindowAddItems
    {
        public delegate void ItemsAddEventHandler(object sender, ItemsAddEventArgs e);
        public event ItemsAddEventHandler ItemsAdded;

        public WindowAddItems()
        {
            InitializeComponent();

            textBoxItemsAdd.EditValueChanged += TextBoxItemsAddEditValueChanged;
            btnItemsAdd.Click += BtnItemsAddClick;
        }

        #region Event Handlers

        private void TextBoxItemsAddEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            btnItemsAdd.IsEnabled = textBoxItemsAdd.Text.Length > 0;
        }
        private void BtnItemsAddClick(object sender, RoutedEventArgs e)
        {
            var filter = new List<string>();
            var pattern = new Regex(@"\b\d{9}\b");

            /*
                        var condition = "";
                        foreach (Match m in pattern.Matches(textBoxItemsAdd.Text))
                        {
                                condition += "^" + m + "$|";
                            filter.Add(m.Value);
                        }
                        if (filter.Count == 0)
                        {
                            MessageBox.Show("Неправильно введён товар", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        condition = condition.TrimEnd('|');
             */



            var condition = "(";
            foreach (Match m in pattern.Matches(textBoxItemsAdd.Text))
            {
                condition += "\'" + m + "\',";
                filter.Add(m.Value);
            }
            if (filter.Count == 0)
            {
                MessageBox.Show("Неправильно введён товар", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            condition = condition.TrimEnd(',') + ")";


            //var lines = textBoxItemsAdd.Text.Split(new[] { '\r', '\n', ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            //var filter = new List<string>();

            //for (int i = 0; i < lines.Length; i++)
            //{
            //    int item;
            //    if (lines[i].Length != 9 || Int32.TryParse(lines[i], out item) == false)
            //    {
            //        lines[i] = "";
            //        continue;
            //    }
            //    filter.Add(lines[i]);
            //    var value = "\'" + lines[i] + "\'";
            //    condition = i == lines.Length - 1 ? condition + value + ")" : condition + value + ",";
            //}

            textBoxItemsAdd.Text = "";
            ItemsAdded(this, new ItemsAddEventArgs { Condition = condition, ListCondition = filter });
        }

        #endregion
    }
}
