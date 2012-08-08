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
using AssortmentManagement.UserValues;
using DevExpress.Data.Selection;
using DevExpress.Xpf.Grid;
using Table = AssortmentManagement.Model.Table;

namespace AssortmentManagement.Windows
{
    /// <summary>
    /// Логика взаимодействия для WindowAssortmentCopy.xaml
    /// </summary>
    public partial class WindowAssortmentCopy : Window
    {
        private readonly Session _session;
        private readonly GridControlRowsGetter _rowGetter;
        private readonly SortedSet<IL> _setIL;
        public event EventHandler DataSourceUpdated;

        public WindowAssortmentCopy(Session session, SortedSet<IL> setIL)
        {
            InitializeComponent();

            _session = session;
            _rowGetter = new GridControlRowsGetter(gridControl1);
            _setIL = setIL;

            Loaded += WindowAssortmentCopyLoaded;
        }

        private void WindowAssortmentCopyLoaded(object sender, RoutedEventArgs e)
        {
            var docType = _session.GetDocument().DocType;

            _session.Fill(docType == DocTypes.ExpendMaterial ? Table.TableLocSourceExpendMaterial : Table.TableLocSource);

            var dimensions = _session.GetDbManager().GetTableDefinition(Table.TableLocSource.DBName);

            //gridControl1.AutoPopulateColumns = true;
            gridControl1.DataSource = _session.GetTableData(Table.TableLocSource);
            //foreach (var column in gridControl1.Columns)
            //{
            //    column.FilterPopupMode = FilterPopupMode.CheckedList;
            //}

            foreach (var dimension in dimensions)
            {
                gridControl1.Columns.Add(new GridColumn { FieldName = dimension.Name, Header = dimension.Desc, FilterPopupMode = FilterPopupMode.CheckedList });
            }
        }

        private void DocumentCreate()
        {
            var toLocs = _rowGetter.GetColumnValuesOfVisibleRows("LOC");
            var fromLoc = _session.GetDbManager().DataTableGetLocsByIL(_setIL).First();
            IEnumerable<string> items = _session.GetDbManager().DataTableGetItemsByIL(_setIL);

            foreach (var toLoc in toLocs)
            {
                _session.GetDbManager().DataTableSecSourceCopy(items, fromLoc, toLoc);
            }

            DataSourceUpdated(this, null);
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DocumentCreate();
        }
    }
}
