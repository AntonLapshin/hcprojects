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
using AssortmentManagement.Model;

namespace AssortmentManagement.Windows
{
    /// <summary>
    /// Логика взаимодействия для WindowChainNative.xaml
    /// </summary>
    public partial class WindowChainNative : Window
    {
        private readonly DBManager _db;
        private List<Chain> _chainGroup;

        public WindowChainNative(Object dbObject, List<Chain> chainGroup)
        {
            InitializeComponent();

            _chainGroup = chainGroup;
            WindowState = WindowState.Maximized;

            _db = (DBManager)dbObject;
            dataGrid1.ItemsSource = _db.DataTableGet("chain").DefaultView;

            //dataGrid1.
        }
    }
}
