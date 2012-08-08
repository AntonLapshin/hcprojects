using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DevExpress.Xpf.Grid;
using NormManagementMVVM.Model;
using SharedComponents.Connection;
using SharedComponents.UI;

namespace NormManagementMVVM.Windows
{
    /// <summary>
    /// Логика взаимодействия для WindowEquipStore.xaml
    /// </summary>
    public partial class WindowEquipStore : ISaved
    {
        private ObservableCollection<Y_NORM_EQUIP_STORE> _addedEquipStore;
        private ObservableCollection<Y_NORM_EQUIP_STORE> _equipStoreCollection;
        private Button _btnAddEquipStore;
        private Button _btnDeleteEquipStore;
        private Button _btnSave;
        private Button _btnExit;
        public WindowEquipStore()
        {
            InitializeComponent();
        }

        #region ISaved Members

        public bool IsSaved { get; set; }

        #endregion

        private new void PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = equipStoreGridControl.GetFocusedRow();
            if (_addedEquipStore.Contains(row))
            {
                if (((TableView)equipStoreGridControl.View).FocusedColumn.FieldName.Equals("STORE") ||
                    ((TableView)equipStoreGridControl.View).FocusedColumn.FieldName.Equals("ID_EQUIP") ||
                    ((TableView)equipStoreGridControl.View).FocusedColumn.FieldName.Equals("STORE1.STORE_NAME") ||
                    ((TableView)equipStoreGridControl.View).FocusedColumn.FieldName.Equals("Y_NORM_EQUIP_TYPE.DESCRIPTION"))
                {
                    WindowDirectoryParameters winDirectoryParams =
                        ((TableView)equipStoreGridControl.View).FocusedColumn.FieldName.Equals("STORE") ||
                        ((TableView)equipStoreGridControl.View).FocusedColumn.FieldName.Equals("STORE1.STORE_NAME")
                            ? new WindowDirectoryParameters(21, false)
                            : new WindowDirectoryParameters(23, false);
                    winDirectoryParams.DirectoryParamSelected += WinDirectoryParamsDirectoryParamSelected;
                    winDirectoryParams.ShowDialog();
                }
            }
        }

        private void WinDirectoryParamsDirectoryParamSelected(object sender, DirectoryParamEventArgs e)
        {
            var row = equipStoreGridControl.GetFocusedRow() as Y_NORM_EQUIP_STORE;
            if (e.Id == 21)
            {
                row.STORE = int.Parse(e.ParameterValues.VALUE);
            }
            if (e.Id == 23)
            {
                row.ID_EQUIP = int.Parse(e.ParameterValues.VALUE);
            }
        }

        private void BtnAddClick(object sender, RoutedEventArgs e)
        {
            var equipStore = new Y_NORM_EQUIP_STORE {CREATE_DATETIME = DateTime.Now, CREATE_ID = User.Name.ToUpper()};
            _equipStoreCollection.Add(equipStore);
            _addedEquipStore.Add(equipStore);
            equipStoreGridControl.View.FocusedRow = equipStore;
        }

        private void BtnRemoveClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var row = equipStoreGridControl.GetFocusedRow() as Y_NORM_EQUIP_STORE;
                if (row == null) throw new ArgumentException("Строка не выбрана!");
                MessageBoxResult result = MessageBox.Show("Вы действительно желаете удалить профиль?",
                                                          "Удаление профиля",
                                                          MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    _equipStoreCollection.Remove(row);
                    if (row.EntityKey == null)
                    {
                        _addedEquipStore.Remove(row);
                    }
                    else
                    {
                        GenericRepository.Delete(row);
                    }
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            _equipStoreCollection = GenericRepository.GetAllObservableCollection<Y_NORM_EQUIP_STORE>();
            equipStoreGridControl.ItemsSource = _equipStoreCollection;
            _addedEquipStore = new ObservableCollection<Y_NORM_EQUIP_STORE>();

            ((TableView)equipStoreGridControl.Columns["STANDARD"].View).InvalidRowException += View_InvalidRowException;

            equipStoreGridControl.View.PreviewMouseDoubleClick += PreviewMouseDoubleClick;
            Closing += WindowEquipStoreClosing;
            InitializeMenuButtons();
        }

        private void InitializeMenuButtons()
        {
            _btnAddEquipStore = UIHelper.FindChild<Button>(equipStoreGridControl, "btnAddEquipStore");
            _btnDeleteEquipStore = UIHelper.FindChild<Button>(equipStoreGridControl, "btnDeleteEquipStore");
            _btnSave = UIHelper.FindChild<Button>(equipStoreGridControl, "btnSave");
            _btnExit = UIHelper.FindChild<Button>(equipStoreGridControl, "btnExit");
            _btnAddEquipStore.Click += BtnAddClick;
            _btnDeleteEquipStore.Click += BtnRemoveClick;
            _btnExit.Click += BtnCloseClick;
            _btnSave.Click += BtnOkClick;
        }

        private void View_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.Ignore;
        }
        private void BtnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnOkClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Save();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при сохранении", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Save()
        {
            try
            {
                var duplicates = _equipStoreCollection.GroupBy(x => new {x.STORE, x.ID_EQUIP}).Where(y => y.Count() > 1);
                if (duplicates.Any())
                {
                    throw new ArgumentException("Существует дублирующие строки");
                }
                foreach (Y_NORM_EQUIP_STORE equip in _addedEquipStore)
                {
                    GenericRepository.Add(equip);
                }
                GenericRepository.UnitOfWork.BeginTransaction();
                GenericRepository.UnitOfWork.CommitTransaction();
                _addedEquipStore.Clear();
                equipStoreGridControl.RefreshData();
                IsSaved = true;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при сохранении", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private bool GetChanges()
        {
            return GenericRepository.GetAll<Y_NORM_EQUIP_STORE>().Any(y => y.EntityState != EntityState.Unchanged) ||
                   _equipStoreCollection.Any(y => y.EntityState != EntityState.Unchanged);
        }

        private void WindowEquipStoreClosing(object sender, CancelEventArgs e)
        {
            if (GetChanges())
            {
                try
                {
                    MessageBoxResult result = MessageBox.Show("Желаете ли вы сохранить текущие изменения?",
                                                              "Сохранимся?",
                                                              MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            Save();
                            break;
                        case MessageBoxResult.No:
                            GenericRepository.RollbackContext();
                            break;
                        case MessageBoxResult.Cancel:
                            e.Cancel = true;
                            break;
                    }
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка при сохранении", MessageBoxButton.OK, MessageBoxImage.Warning);
                    e.Cancel = true;
                }
            }
        }
    }
}