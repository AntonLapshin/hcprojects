using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using AssortmentManagement.UserValues;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.PivotGrid;
using AssortmentManagement.Model;

namespace AssortmentManagement.Windows
{
    public partial class WindowBase
    {
        #region Fields

        private readonly Session _session;
        private readonly BackgroundWorker _worker;
        private string _clauseCondition;

        #endregion

        public WindowBase(Session session)
        {
            InitializeComponent();

            #region Initialize

            _session = session;

            WindowState = WindowState.Maximized;
            //ThemeManager.SetThemeName(this, "Office2007Blue");
            //ThemeManager.SetThemeName(this, "Office2007Black");
            //ThemeManager.SetThemeName(this, "LightGray");
            ThemeManager.SetThemeName(this, "DeepBlue");
            _session.GetDocument().DocProjected += DbDocProjected; // ???
            Closing += WindowBaseClosing;

            #endregion

            #region Initialize Background Worker

            _worker = new BackgroundWorker();
            _worker.DoWork += WorkerDoWork;
            _worker.RunWorkerCompleted += WorkerRunWorkerCompleted;
            _worker.ProgressChanged += WorkerProgressChanged;

            #endregion

            #region Initialize PivotGrid Control

            _pivotGridControl1.DataSource = _session.GetTableData(Table.TableBaseSource);
            _pivotGridControl1.HiddenFieldList += PivotGridControl1HiddenFieldList;

            List<Column> columns;

            try
            {
                columns = _session.GetTableColumns(Table.TableSecSource);
            }
            catch (AssortmentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _pivotGridControl1.InitializeControl(columns, FormTypes.Base);

            _pivotGridControl1.Fields["LOC"].Area = FieldArea.ColumnArea;
            _pivotGridControl1.Fields["LOC"].Visible = true;
            _pivotGridControl1.Fields["ITEM"].Area = FieldArea.RowArea;
            _pivotGridControl1.Fields["ITEM"].Visible = true;
            _pivotGridControl1.Fields["MEASURE_STATUS_NEW"].Area = FieldArea.DataArea;
            _pivotGridControl1.Fields["MEASURE_STATUS_NEW"].Visible = true;

            _pivotGridControl1.ShowRowTotals = false;
            _pivotGridControl1.ShowColumnTotals = false;

            #endregion
        }

        #region Event Handlers

        private void WindowValuesItemsValuesSelected(object sender, ValuesEventArgs e)
        {
            if (e.Result == false)
            {
                MessageBox.Show("Ошибка фильтрации: " + e.Error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _pivotGridControl1.SetFilters("ITEM", e.Values);
            _pivotGridControl1.RefreshData();
        }

        private void DbDocProjected(object sender, EventArgs e)
        {
            BeginInit();
            _pivotGridControl1.RefreshData();
            EndInit();
        }
        private void WindowBaseClosing(object sender, CancelEventArgs e)
        {
            if (_worker.IsBusy)
            {
                MessageBox.Show("Вы не можете закрыть рабочее окно в данный момент", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                e.Cancel = true;
                return;
            }
            if (MessageBox.Show("Вы действительно хотите закрыть рабочее окно?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }
        private void GridInfoMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            gridInfo.Visibility = Visibility.Hidden;
        }
        private void PivotGridControl1HiddenFieldList(object sender, RoutedEventArgs e)
        {
            menuItemFieldsList.Opacity = 0.5;
        }

        #endregion

        #region Background Worker Methods

        private void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;

            if (worker == null)
            {
                MessageBox.Show("Background Worker is null", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Result = InitializeResults.Error;
                return;
            }
            worker.WorkerReportsProgress = true;
            worker.ReportProgress(0);

            Thread.Sleep(10);

            try
            {
                _session.InitializeSec(_clauseCondition);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Result = InitializeResults.Error;
                return;
            }

            worker.ReportProgress(1);

            try
            {
                _session.Fill(Table.TableSecSource);
                //_session.Fill(_session.GetDocument().DocType == DocTypes.ExpendMaterial ? Table.TableSecSourceExtendMaterial : Table.TableSecSource);
            }
            catch (AssortmentException ex)
            {
                MessageBox.Show(Table.TableSecSource + ": " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Result = InitializeResults.Error;
                return;
            }

            Thread.Sleep(10);
            e.Result = InitializeResults.Successful;
        }
        private void WorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Cursor = Cursors.Arrow;

            if (e.Error != null)
            {
                MessageBox.Show("Ошибка инициализации: " + e.Error.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if ((InitializeResults)e.Result == InitializeResults.Successful)
                {
                    labelInfo.Content = "Окно не активно";

                    _pivotGridControl1.HideFieldList();

                    var window = new WindowSecondary(_session, _pivotGridControl1);
                    window.ShowDialog();
                    gridInfo.Visibility = Visibility.Hidden;
                    ((UIElement)Content).IsEnabled = true;
                }
                else
                {
                    labelInfo.Content = "Ошибка инициализации";
                    ((UIElement)Content).IsEnabled = true;
                }
            }
        }
        private void WorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (e.ProgressPercentage)
            {
                case 0:
                    {
                        labelInfo.Content = "Идёт инициализация временной таблицы...";
                        break;
                    }
                case 1:
                    {
                        labelInfo.Content = "Идёт формирование данных...";
                        break;
                    }
            }
        }

        #endregion

        #region Menu Items Methods

        private void MenuItemFiltersItems(object sender, RoutedEventArgs e)
        {
            var windowValuesItems = new WindowSelectValues();
            windowValuesItems.ValuesSelected += WindowValuesItemsValuesSelected;
            windowValuesItems.ShowDialog();
        }

        private void MenuItemFieldsListClick(object sender, RoutedEventArgs e)
        {
            if (_pivotGridControl1.IsFieldListVisible)
            {
                menuItemFieldsList.Opacity = 0.5;
                _pivotGridControl1.HideFieldList();
            }
            else
            {
                menuItemFieldsList.Opacity = 1;
                _pivotGridControl1.ShowFieldList();
            }
        }
        private void MenuItemSummaryClick(object sender, RoutedEventArgs e)
        {
            _pivotGridControl1.ShowRowTotals = !_pivotGridControl1.ShowRowTotals;
            _pivotGridControl1.ShowColumnTotals = !_pivotGridControl1.ShowColumnTotals;
        }
        private void MenuItemSummaryTotalClick(object sender, RoutedEventArgs e)
        {
            _pivotGridControl1.ShowColumnGrandTotals = !_pivotGridControl1.ShowColumnGrandTotals;
            _pivotGridControl1.ShowRowGrandTotals = !_pivotGridControl1.ShowRowGrandTotals;
        }

        private void MenuItemRegularDocCreateClick(object sender, RoutedEventArgs e)
        {
            _session.CreateDocument(DocTypes.Regular);
            MenuItemDocCreateClick();
        }

        private void MenuItemOperativeDocCreateClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите создать оперативный документ?",
                                                     "Подтверждение",
                                                      MessageBoxButton.YesNo,
                                                      MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _session.CreateDocument(DocTypes.Operative);
                MenuItemDocCreateClick();
            }
        }

        private void MenuItemExpendMaterialDocClick(object sender, RoutedEventArgs e)
        {
            // Проверка на существование товаров расходников в y_assortment_item_gtt

            if (MessageBox.Show("Вы действительно хотите создать документ \"Расходники склад\"?",
                                                     "Подтверждение",
                                                      MessageBoxButton.YesNo,
                                                      MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _session.CreateDocument(DocTypes.ExpendMaterial);
                MenuItemDocCreateClick();
            }
        }

        private void MenuItemDocCreateClick()
        {
            ((UIElement)Content).IsEnabled = false;

            Cursor = Cursors.Wait;

            #region Initialize Secondary Source (Start Background Worker)

            gridInfo.Visibility = Visibility.Visible;

            _clauseCondition =
                _pivotGridControl1.GetCondition(new List<FieldArea>
                                                                    {
                                                                        FieldArea.ColumnArea,
                                                                        FieldArea.RowArea,
                                                                        FieldArea.DataArea,
                                                                        FieldArea.FilterArea
                                                                    });

            if (!_worker.IsBusy)
                _worker.RunWorkerAsync();
            else
            {
                MessageBox.Show("Документ уже формируется", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            #endregion
        }


        private void MenuItemRegisterClick(object sender, RoutedEventArgs e)
        {
            var windowRegister = new WindowRegister(_session);
            windowRegister.ShowDialog();
        }

        #endregion
    }
}
