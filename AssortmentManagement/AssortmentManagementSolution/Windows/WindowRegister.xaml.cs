using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using AssortmentManagement.Controls;
using AssortmentManagement.Model;
using AssortmentManagement.UserValues;
using DevExpress.Utils;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;

namespace AssortmentManagement.Windows
{
    public partial class WindowRegister
    {
        #region Fields

        private readonly Session _session;

        #endregion

        //private readonly DBManager _db;
        private readonly BackgroundWorker _worker;
        //private readonly PivotGridControlModified _pivotGridControl1;
        //private readonly Document _doc;

        //public WindowRegister(Object dbObject, Object pivotGridControl, Document doc)
        public WindowRegister(Session session)
        {
            InitializeComponent();

            Width = 880;
            Height = 720;

            _session = session;

            //_pivotGridControl1 = (PivotGridControlModified)pivotGridControl;
            //_db = (DBManager)dbObject;
            //_doc = doc;

            #region Initialize Background Worker

            _worker = new BackgroundWorker();
            _worker.DoWork += WorkerDoWork;
            _worker.RunWorkerCompleted += WorkerRunWorkerCompleted;
            _worker.ProgressChanged += WorkerProgressChanged;

            #endregion

            #region Initialize Control

            try
            {
                _session.FillRegister();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }

            gridControl1.DataSource = _session.GetTableData(Table.TableRegister);

            gridControl1.Columns.Add(new GridColumn { FieldName = "ID", Header = "Номер", AllowEditing = DefaultBoolean.False, AllowBestFit = DefaultBoolean.True, Visible = true });
            gridControl1.Columns.Add(new GridColumn { FieldName = "ID_USER", Header = "Пользователь", AllowEditing = DefaultBoolean.False, AllowBestFit = DefaultBoolean.True, CellTemplate = (DataTemplate)FindResource("RowTemplate") });
            gridControl1.Columns.Add(new GridColumn { FieldName = "CREATE_TIME", Header = "Дата создания", AllowEditing = DefaultBoolean.False, AllowBestFit = DefaultBoolean.True, CellTemplate = (DataTemplate)FindResource("RowTemplate") });
            gridControl1.Columns.Add(new GridColumn { FieldName = "ROW_COUNT", Header = "Количество строк", AllowEditing = DefaultBoolean.False, AllowBestFit = DefaultBoolean.True, CellTemplate = (DataTemplate)FindResource("RowTemplate") });
            gridControl1.Columns.Add(new GridColumn { FieldName = "STATUS", Header = "Статус", AllowEditing = DefaultBoolean.False, AllowBestFit = DefaultBoolean.True, CellTemplate = (DataTemplate)FindResource("RowTemplate") });
            gridControl1.Columns.Add(new GridColumn { FieldName = "DOC_TYPE", Header = "Тип", AllowEditing = DefaultBoolean.False, AllowBestFit = DefaultBoolean.True, CellTemplate = (DataTemplate)FindResource("RowTemplate") });
            gridControl1.Columns.Add(new GridColumn { FieldName = "LAST_UPDATE_TIME", Header = "Дата обновления", AllowEditing = DefaultBoolean.False, AllowBestFit = DefaultBoolean.True, CellTemplate = (DataTemplate)FindResource("RowTemplate") });
            gridControl1.Columns.Add(new GridColumn { FieldName = "DESCRIPTION", Header = "Комментарий", AllowEditing = DefaultBoolean.False, AllowBestFit = DefaultBoolean.True, CellTemplate = (DataTemplate)FindResource("RowTemplate") });

            gridControl1.SortBy(gridControl1.Columns["ID"]);

            gridControl1.MouseDoubleClick += GridControl1MouseDoubleClick;
            gridControl1.MouseRightButtonDown += GridControl1MouseRightButtonDown;

            //gridControl1.View.GroupRowTemplate = (DataTemplate)FindResource("RowTemplate");

            #endregion
        }

        #region Background Worker Methods

        private void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            if (worker == null)
            {
                MessageBox.Show("Background Worker is null", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Result = "Внутренняя ошибка";
                return;
            }
            worker.WorkerReportsProgress = true;
            worker.ReportProgress(0);

            Thread.Sleep(10);

            worker.ReportProgress(1);

            try
            {
                _session.Fill(Table.TableSecSource);
            }
            catch (AssortmentException ex)
            {
                MessageBox.Show(Table.TableSecSource + ": " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            /*
                        if (_db.FillDataTableCustom(Table.TableSecSource) == false)
                        {
                            MessageBox.Show("Ошибка при формировании источника: "+_db.Error,"Ошибка",MessageBoxButton.OK,MessageBoxImage.Error);
                        }
            */
            Thread.Sleep(10);
            e.Result = "Successful";
        }
        private void WorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("Ошибка при загрузке документа: " + e.Error.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (e.Result.Equals("Successful"))
                {
                    labelInfo.Content = "Окно не активно";
                    //_pivotGridControl1.HideFieldList();
                    var window =
                        new WindowSecondary(_session);
                    //var window = new WindowSecondary(_session);
                    window.ShowDialog();
                }
                else
                {
                    labelInfo.Content = "Ошибка инициализации";
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
                case -1:
                    {
                        labelInfo.Content = "Ошибка инициализации";
                        break;
                    }
            }
        }

        #endregion

        #region Event Handlers

        private void GridControl1MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            char status = Char.Parse((gridControl1.GetFocusedRowCellValue("STATUS").ToString()).Split('&')[1]);
            if (status.Equals('A'))
            {
                MessageBox.Show("Документ акцептован. Удаление невозможно.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (status.Equals('R'))
            {
                MessageBox.Show("Документ в статусе R (READY). Удаление невозможно.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (status.Equals('M'))
            {
                MessageBox.Show("Документ в статусе M. Удаление невозможно.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var description = (gridControl1.GetFocusedRowCellValue("DESCRIPTION").ToString()).Split('&')[0];
            if (MessageBox.Show("Вы уверены что хотите удалить документ \"" + description + "\"", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (_session.GetDocument().Delete(Convert.ToInt32(gridControl1.GetFocusedRowCellValue("ID"))) == true)
                    Update();
            }
        }

        private void GridControl1MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            char status = Char.Parse((gridControl1.GetFocusedRowCellValue("STATUS").ToString()).Split('&')[1]);
            if (status.Equals('A'))
            {
                MessageBox.Show("Документ акцептован. Редактирование невозможно.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (status.Equals('R'))
            {
                MessageBox.Show("Документ в статусе R (READY). Редактирование невозможно.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (status.Equals('M'))
            {
                MessageBox.Show("Документ в статусе M. Редактирование невозможно.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                _session.GetDocument().Open(Convert.ToInt32(gridControl1.GetFocusedRowCellValue("ID")));
            }
            catch (AssortmentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            gridInfo.Visibility = Visibility.Visible;
            if (!_worker.IsBusy)
                _worker.RunWorkerAsync();
            else
            {
                MessageBox.Show("Документ уже запускается", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void GridInfoMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            gridInfo.Visibility = Visibility.Hidden;

            // refresh register data table
            Update();
        }

        public void Update()
        {
            try
            {
                _session.FillRegister();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
            gridControl1.RefreshData();
        }

        #endregion

        private void ButtonGlobalCheckClick(object sender, RoutedEventArgs e)
        {
            labelInfo.Content = "Нажмите для продолжения...";
            gridInfo.Visibility = Visibility.Visible;
            var windowCheck = new WindowCheck(_session, CheckTypes.Global);
#pragma warning disable 612,618
            DXGridDataController.DisableThreadingProblemsDetection = true;
#pragma warning restore 612,618
            windowCheck.ShowDialog();
        }
    }
}
