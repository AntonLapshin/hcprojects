using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AssortmentManagement.Model;
using AssortmentManagement.UserValues;
using Table = AssortmentManagement.Model.Table;

namespace AssortmentManagement.Windows
{
    /// <summary>
    /// Логика взаимодействия для WindowWhRowEdit.xaml
    /// </summary>
    public partial class WindowWhRowEdit : Window
    {
        private ChainRec _row;
        private readonly DBManager _db;

        public WindowWhRowEdit(Object dbobject, ChainRec row)
        {
            InitializeComponent();
            _db = (DBManager)dbobject;
            _row = row;

            try
            {
                _db.FillDataTableCustom(Table.TableSupplier.Name, Table.TableSupplier.SelectClause, Table.TableSupplier.KeyFields, false);
            }
            catch (AssortmentException ex)
            {
                MessageBox.Show(Table.TableSupplier + ": " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
/*
            if (_db.FillDataTableCustom(Table.TableSupplier.Name, Table.TableSupplier.SelectClause, Table.TableSupplier.KeyFields, false) == false)
            {
                MessageBox.Show("Ошибка при формировании источника: "+_db.Error,"Ошибка",MessageBoxButton.OK,MessageBoxImage.Error);
            }
*/
            #region Initialize Events

            sourceMethodNew.SelectionChanged += SourceMethodNewSelectionChanged;
            cancel.Click += CancelClick;
            supplierNewSelect.Click += SupplierNewSelectClick;

            border1.MouseEnter += BorderMouseEnter;
            border1.MouseLeave += BorderMouseLeave;
            border2.MouseEnter += BorderMouseEnter;
            border2.MouseLeave += BorderMouseLeave;
            border3.MouseEnter += BorderMouseEnter;
            border3.MouseLeave += BorderMouseLeave;

            #endregion

            #region Initialize Controls

            sourceMethodNew.Items.Add((char)SourceMethods.S);
            sourceMethodNew.Items.Add((char)SourceMethods.W);

            for (var i = 0; i < _db.UserWhList.Count; i++)
            {
                sourceWhNew.Items.Add(_db.UserWhList[i]);
            }
            //sourceWhNew.Items.Add(44);
            //sourceWhNew.Items.Add(121);
            //sourceWhNew.Items.Add(174);

            //sourceMethod.Text = row.SourceMethod.ToString();
            //sourceWh.Text = row.SourceWh.ToString();
            //sourceMethodNew.SelectedItem = row.SourceMethodNew;
            //sourceWhNew.SelectedItem = row.SourceWhNew;
            //supplier.Text = row.Supplier.ToString();
            //supplierDesc.Text = row.SupplierDesc;
            //supplierNew.Text = row.SupplierNew.ToString();
            //supplierDescNew.Text = row.SupplierDescNew;
            //status.Text = row.Status.ToString();
            //statusNew.Text = row.StatusNew.ToString();

            #endregion
        }

        private void SupplierNewSelectClick(object sender, RoutedEventArgs e)
        {
            var windowSupplier = new WindowSupplier(_db);
            windowSupplier.SupplierSelected += WindowSupplierSupplierSelected;
            windowSupplier.ShowDialog();
        }

        private void WindowSupplierSupplierSelected(object sender, SupplierEventArgs e)
        {
            supplierNew.Text = e.Supplier.ToString();
            supplierDescNew.Text = e.Name;
        }

        private void BorderMouseLeave(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = Brushes.White;
        }

        private void BorderMouseEnter(object sender, MouseEventArgs e)
        {
            var brush = new LinearGradientBrush(Colors.LightGreen, Colors.White, 90);
            ((Border)sender).Background = brush;
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SourceMethodNewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Convert.ToChar(sourceMethodNew.SelectedValue) ==(char)SourceMethods.S)
            {
                sourceWhNew.SelectedItem = null;
            }
        }
    }
}
