using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using AssortmentManagement.Model;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Media;
using AssortmentManagement.UserValues;

namespace AssortmentManagement.Windows
{
    public partial class WindowAuthorization
    {
        #region Fields

        private Session _session;

        private readonly BackgroundWorker _worker;
        private readonly SortedList<string, string> _merchList;
        private bool _testDb;

        #endregion

        public WindowAuthorization()
        {
            InitializeComponent();
            labelVersion.Content = "Версия: " + new DateTime(2000, 1, 1).AddDays(Application.ResourceAssembly.GetName().Version.Build).ToShortDateString();

            #region Preliminary Connection for merch list load

            using (var db = new DBManager())
            {
                //var db = new DbManagerDynamic("rmstst", "rmstst", true);

                try
                {
                    db.ConnectionOpen("rmsprd", "golive104");
                }
                catch (ConnectionException e)
                {
                    MessageBox.Show(e.Message, "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                    return;
                }
                catch (AuthorizationException e)
                {
                    MessageBox.Show(e.Message, "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                    return;
                }

                try
                {
                    _merchList = Merchant.GetMerchList(db);
                }
                catch (AssortmentException e)
                {
                    MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                    return;
                }
                
                listLogin.ItemsSource = _merchList.Keys;
                listLogin.SelectedIndex = 0;

                db.ConnectionClose();

            }

            #endregion

            #region Initialize Background Worker

            _worker = new BackgroundWorker();
            _worker.DoWork += WorkerDoWork;
            _worker.RunWorkerCompleted += WorkerRunWorkerCompleted;
            _worker.ProgressChanged += WorkerProgressChanged;
            _worker.WorkerSupportsCancellation = true;

            #endregion
        }

        #region Event Handlers

        private void MainWindowClosed(object sender, EventArgs e)
        {
            //if (MessageBox.Show("Желаете ли вы сохранить данную сессию для последующей быстрой загрузки?",
            //                    "Сохранение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            //{
            //    try
            //    {
            //        _session.Backup(ExitCodes.Successful);
            //    }
            //    catch (AssortmentException ex)
            //    {
            //        MessageBox.Show("Произошла ошибка при сохранении сессии: " + ex.Message, "Ошибка", MessageBoxButton.OK,
            //                        MessageBoxImage.Error);
            //    }
            //}

            Close();
        }
        private void BtnSubmitClick(object sender, RoutedEventArgs e)
        {
            Authorization();
        }
        private void WindowKeyDown(object sender, KeyEventArgs e)
        {
            if (_worker.IsBusy) return;
            if (e.Key == Key.Enter)
            {
                Authorization();
            }

            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.T))
            {
                imageLogo.Opacity = imageLogo.Opacity == 1 ? 0.3 : 1;

            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.R))
            {
                if (!_testDb)
                {
                    labelName.Content = "Тестовый сервер";
                    labelName.Foreground = Brushes.DarkBlue;
                }
                else
                {
                    labelName.Content = "Рабочий сервер";
                    labelName.Foreground = Brushes.Black;
                }
                _testDb = !_testDb;
            }
        }
        private void Rectangle1MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void Image1MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_session == null)
            {
                Close();
            }
            else
            {
                if (_session.GetState() == SessionStates.Active)
                {
                    labelStatus.Text = "Идёт отмена операции...";
                    _session.Stop();
                }
                else
                {
                    Close();
                }
            }
        }
        private void WindowAuthorizationClosed(object sender, EventArgs e)
        {
            if (_session != null) _session.End();
        }

        #endregion

        #region Background Worker Methods

        private void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            if (worker == null)
            {
                MessageBox.Show("Background Worker is null", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Result = InitializeResults.Error;
                return;
            }
            worker.WorkerReportsProgress = true;
            Thread.Sleep(10);

            try
            {
                _session.Start();
            }
            catch (ConnectionException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Result = InitializeResults.Error;
                return;
            }
            catch (AuthorizationException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                e.Result = InitializeResults.Error;
                return;
            }
            catch (AssortmentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка получения списка доступных подразделений", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                e.Result = InitializeResults.Error;
                return;
            }

            UnhandledExceptionHandler.Db = _session.GetDbManager();
            UnhandledExceptionHandler.Initialize();

            worker.ReportProgress(1);
            Thread.Sleep(10);
            worker.ReportProgress(2);

            string login;
            ExitCodes res;

            //try
            //{
            //    _session.BackupCheck(out login, out res);
            //}
            //catch (AssortmentException ex)
            //{
            //    MessageBox.Show("Произошла ошибка при подготовке к инициализации: " + ex.Message, "Ошибка", MessageBoxButton.OK,
            //                    MessageBoxImage.Error);
            //    e.Result = InitializeResults.Error;
            //    return;
            //}

            //if (login == _session.GetLogin())
            //{
            //    var result = res == ExitCodes.Exception
            //                     ? MessageBox.Show(
            //                         "Предыдущий запуск завершился неудачей. Желаете ли восстановить данные предыдущего запуска?",
            //                         "Восстановление", MessageBoxButton.YesNo, MessageBoxImage.Question)
            //                     : MessageBox.Show("Желаете ли вы загрузить сохраненную сессию?", "Загрузка",
            //                                       MessageBoxButton.YesNo, MessageBoxImage.Question);
            //    if (result == MessageBoxResult.Yes)
            //    {
            //        // Restore from backup
            //        try
            //        {
            //            _session.Restore();
            //            worker.ReportProgress(3);
            //            Thread.Sleep(10);
            //            e.Result = InitializeResults.Successful;
            //        }
            //        catch (CancelException)
            //        {
            //            e.Result = InitializeResults.Cancel;
            //        }
            //        catch (AssortmentException ex)
            //        {
            //            MessageBox.Show("Произошла ошибка при восстановлении данных: " + ex.Message, "Ошибка", MessageBoxButton.OK,
            //                            MessageBoxImage.Error);
            //            e.Result = InitializeResults.Error;
            //        }
            //        return;
            //    }
            //}
            // Initialize
            try
            {
                _session.InitializeBase();

                worker.ReportProgress(3);
                Thread.Sleep(10);
                e.Result = InitializeResults.Successful;
            }
            catch (CancelException)
            {
                e.Result = InitializeResults.Cancel;
            }
            catch (AssortmentException ex)
            {
                MessageBox.Show("Ошибка при инициализации: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Result = InitializeResults.Error;
            }
        }
        private void WorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("Ошибка при инициализации: " + e.Error.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            labelStatus.Text = ((InitializeResults)e.Result).Description();

            if ((InitializeResults)e.Result == InitializeResults.Successful)
            {
                Table.TableBaseSource.SelectClause += " where loc in (" +
                                                      string.Join(",", _session.UserStoreList.Keys) +
                                                      (_session.UserWhList.Count == 0
                                                           ? ""
                                                           : ","
                                                             + string.Join(",", _session.UserWhList.Keys))
                                                      + ")";

                try
                {
                    _session.Fill(Table.TableBaseSource);
                }
                catch (AssortmentException ex)
                {
                    MessageBox.Show(Table.TableBaseSource + ": " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                //Window mainWindow = new WindowBase(_session.GetDbManager());
                Window mainWindow = new WindowBase(_session);
                mainWindow.Closed += MainWindowClosed;

                labelStatus.Text = "Инициализация выполнена";
                Hide();
                mainWindow.Show();
                return;
            }
            listLogin.IsEnabled = true;
            txtPassword.IsEnabled = true;
            btnSubmit.IsEnabled = true;
        }
        private void WorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (e.ProgressPercentage)
            {
                case 0:
                    {
                        labelStatus.Text = "Соединение установлено";
                        break;
                    }
                case 1:
                    {
                        labelStatus.Text = _session.GetMerchName();
                        break;
                    }
                case 2:
                    {
                        labelStatus.Text = "Идёт инициализация глобальных временных таблиц...";
                        break;
                    }
                case 3:
                    {
                        labelStatus.Text = "Идёт формирование данных...";
                        break;
                    }
            }
        }

        #endregion

        private void Authorization()
        {
            listLogin.IsEnabled = false;
            txtPassword.IsEnabled = false;
            btnSubmit.IsEnabled = false;
            labelStatus.Text = "Авторизация...";
            labelStatus.Visibility = Visibility.Visible;

            var data = imageLogo.Opacity == 1 ? OperationModes.Production : OperationModes.Test;

            _session = _testDb ?
                      new Session("ATRUSHINA", "atrushina", data, OperationModes.Test)
                    : new Session(_merchList[listLogin.SelectedValue.ToString()], txtPassword.Password, data, OperationModes.Production);


            if (!_worker.IsBusy)
            {
                _worker.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("Идёт формирование таблиц", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
