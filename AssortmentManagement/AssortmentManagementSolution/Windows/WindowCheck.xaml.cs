using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AssortmentManagement.Model;
using AssortmentManagement.UserValues;

namespace AssortmentManagement.Windows
{
    public partial class WindowCheck
    {
        private readonly DBManager _db;
        //private readonly Document _doc;
        private readonly Session _session;
        private readonly BackgroundWorker _worker;
        private readonly List<Check> _checkList;
        private int _nCheck;
        private readonly CheckTypes _checkType;

        public WindowCheck(Session session, CheckTypes checkType)
        {
            InitializeComponent();

            #region Initialize

            _session = session;
            _db = session.GetDbManager();
            _checkType = checkType;

            listBoxCheck.MouseDoubleClick += ListBoxCheckMouseDoubleClick;
            listBoxCheck.SelectionChanged += ListBoxCheckSelectionChanged;
            buttonCancel.Click += ButtonCancelClick;
            buttonSubmit.Click += ButtonSubmitClick;
            _session.GetDocument().DocProjected += DbDocProjected;

            _checkList = new List<Check>();

            try
            {
                _checkList.AddRange(_session.GetDocument().GetCheckList(_checkType));
            }
            catch (AssortmentDBException e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            listBoxCheck.ItemsSource = _checkList;

            #endregion

            #region Initialize Background Worker

            _worker = new BackgroundWorker();
            _worker.DoWork += WorkerDoWork;
            _worker.RunWorkerCompleted += WorkerRunWorkerCompleted;
            _worker.ProgressChanged += WorkerProgressChanged;

            #endregion

            if (_checkList.Count == 0) return;

            _checkList[0].Status = CheckStatuses.Executing;
            _nCheck = 0;
            if (!_worker.IsBusy)
                _worker.RunWorkerAsync();
            else
            {
                MessageBox.Show("Проверки уже запущены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DbDocProjected(object sender, EventArgs e)
        {
            Close();
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

            for (int nCheck = 0; nCheck < _checkList.Count; nCheck++)
            {
                var check = _checkList[nCheck];
                CheckStatuses checkStatus;
                try
                {
                    checkStatus = (CheckStatuses)_session.GetDocument().Check(check.N);
                }
                catch (CancelException)
                {
                    checkStatus = CheckStatuses.Cancel;
                }
                catch (AssortmentDBException ex)
                {
                    MessageBox.Show("Ошибка при выполнении проверки \"" + check.Desc + "\": " + ex.Message, "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Stop);
                    checkStatus = CheckStatuses.Exception;
                }

                if (check.TableName == null)
                {
                    Console.WriteLine("null");
                }

                switch (_checkType)
                {
                    case CheckTypes.Local:
                        {
                            if (checkStatus == CheckStatuses.Error || checkStatus == CheckStatuses.Warning)
                            {
                                string query = "";
                                string[] key;
                                if (check.TableName != null)
                                {
                                    query = "select * from " + check.TableName;
                                    key = new []{ "LOC" };
                                }
                                else
                                {
                                    query = checkStatus == CheckStatuses.Error
                                                    ? "select d.*, sec.dim_item_desc, sec.dim_loc_desc, sec.dim_itemloc_supplier_desc, sec.dim_itemloc_supplier_desc_new from y_assortment_doc_detail d join y_assortment_united_sec_gtt sec on sec.item = d.item and sec.loc = d.loc where d.check_result = 'E' and d.id = " +
                                                      _session.GetDocument().Id
                                                    : "select d.*, sec.dim_item_desc, sec.dim_loc_desc, sec.dim_itemloc_supplier_desc, sec.dim_itemloc_supplier_desc_new from y_assortment_doc_detail d join y_assortment_united_sec_gtt sec on sec.item = d.item and sec.loc = d.loc where d.check_result = 'W' and d.id = " +
                                                      _session.GetDocument().Id;
                                    key = new[] { "ID" };
                                }
                                try
                                {
                                    _db.FillDataTableCustom(_checkList[nCheck].ProcedureName, query, key);
                                }
                                catch (AssortmentException ex)
                                {
                                    if (ex.Message != "Dynamic SQL generation failed. No key information found")
                                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                                
                            }
                            break;
                        }
                    case CheckTypes.Global:
                        {
                            _db.FillDataTableCustom(_checkList[nCheck].ProcedureName,
                                                    "select * from y_assortment_united_sec_gtt", new[] { "ITEM", "LOC" });
                            break;
                        }
                }

                e.Result = checkStatus;

                if (checkStatus != CheckStatuses.Exception)
                    worker.ReportProgress((int)checkStatus);
                else
                    break;
            }
        }
        private void WorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((CheckStatuses)e.Result == CheckStatuses.Cancel ||
                (CheckStatuses)e.Result == CheckStatuses.Exception) return;
            buttonSubmit.IsEnabled = true;
            foreach (var check in _checkList)
            {
                if (check.Status == CheckStatuses.Error) buttonSubmit.IsEnabled = false;
            }
        }
        private void WorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (_nCheck >= _checkList.Count) return;

            listBoxCheck.BeginInit();

            switch (e.ProgressPercentage)
            {
                case -2:
                    _checkList[_nCheck].Status = CheckStatuses.Cancel;
                    break;
                case 0:
                    _checkList[_nCheck].Status = CheckStatuses.Success;
                    break;
                case 1:
                    _checkList[_nCheck].Status = CheckStatuses.Error;
                    break;
                case 2:
                    _checkList[_nCheck].Status = CheckStatuses.Warning;
                    break;
            }

            if (_checkList.Count > _nCheck + 1)
                _checkList[++_nCheck].Status = CheckStatuses.Executing;

            listBoxCheck.EndInit();
        }

        #endregion

        private void ListBoxCheckSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxCheck.SelectedItem == null) return;

            listBoxCheck.BeginInit();
            foreach (var check in _checkList)
            {
                check.Selected = false;
            }

            ((Check)listBoxCheck.SelectedItem).Selected = true;
            listBoxCheck.EndInit();
        }
        private void ButtonCancelClick(object sender, RoutedEventArgs e)
        {
            if (_db.Command == null)
            {
                Close();
            }
            else
            {
                _db.Command.Cancel();
                Close();
            }

        }
        private void ButtonSubmitClick(object sender, RoutedEventArgs e)
        {
            switch (_checkType)
            {
                case CheckTypes.Local:
                    {
                        try
                        {
                            _session.GetDocument().Accept();
                            _session.GetDocument().Projection();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка при акцепте: " + ex.Message, "Ошибка", MessageBoxButton.OK,
                                            MessageBoxImage.Error);
                            break;
                        }

                        // refresh base source (doc's changes adding)
                        string query = "select * from " + Table.TableBaseSource.DBName +
                                       " u where exists (select 1 from " + Table.TableDocDetail.DBName + " doc where doc.id = " +
                                       _session.GetDocument().Id + " and doc.item = u.item and doc.loc = u.loc)";

                        try
                        {
                            _db.FillDataTableCustom(Table.TableBaseSource.Name, query, null, false);
                        }
                        catch(AssortmentException ex)
                        {
                            MessageBox.Show(Table.TableBaseSource + ": " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
/*
                        if (_db.FillDataTableCustom(Table.TableBaseSource.Name, query, null, false) == false)
                        {
                            MessageBox.Show("Ошибка при обновлении источника: " + _db.Error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
*/
                        _session.GetDocument().ToBaseWindow();
                        break;
                    }
                case CheckTypes.Global:
                    {
                        try
                        {
                            _session.GetDocument().Ready();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка при подготовке документов: " + ex.Message, "Ошибка", MessageBoxButton.OK,
                                            MessageBoxImage.Error);
                        }
                        Close();
                        break;
                    }
            }
        }
        private void ListBoxCheckMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listBoxCheck.SelectedItem == null) return;
            if (((Check)listBoxCheck.SelectedItem).Status == CheckStatuses.Error || ((Check)listBoxCheck.SelectedItem).Status == CheckStatuses.Warning)
            {
                if (((Check)listBoxCheck.SelectedItem).ProcedureName.Equals("doc_create"))
                {
                    MessageBox.Show(_db.Error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var windowCheckError = new WindowCheckError(_session, _checkType, (Check)listBoxCheck.SelectedItem);
                    windowCheckError.WhRestExistsCheckNewDoc += WhRestExistsCheckNewDocHandler;
                    windowCheckError.ShowDialog();
                }
            }
        }

        void WhRestExistsCheckNewDocHandler(object sender, EventArgs e)
        {
            Close();
        }
    }
}
