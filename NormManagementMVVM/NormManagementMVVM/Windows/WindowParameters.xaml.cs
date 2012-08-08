using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Grid;
using NormManagementMVVM.Model;
using SharedComponents.Connection;
using SharedComponents.UI;

namespace NormManagementMVVM.Windows
{
    /// <summary>
    /// Логика взаимодействия для WindowParameters.xaml
    /// </summary>
    public partial class WindowParameters : Window, ISaved
    {
        private ObservableCollection<Y_NORM_PARAMETERS> _parametersCollection;
        private Button _btnParamAdd;
        private Button _btnParamDelete;
        private Button _btnSave;
        private Button _btnExit;
        public WindowParameters()
        {
            InitializeComponent();
        }

        #region ISaved Members

        public bool IsSaved { get; set; }

        #endregion

        private void ButtonAddClick(object sender, RoutedEventArgs e)
        {
            var param = new Y_NORM_PARAMETERS
                            {
                                ID = _parametersCollection.Max(y => y.ID) + 1,
                                CREATE_DATETIME = DateTime.Now,
                                CREATE_ID = User.Name.ToUpper()
                            };
            _parametersCollection.Add(param);
            GenericRepository.Add(param);
        }

        private bool IsNotExistsParamInNormativeOrProfile(long id)
        {
            bool temp = true;
            var normCell = GenericRepository.FindOne<Y_NORM_NORMATIVE_CELL>(y => y.ID_PARAM == id);
            var profileDetail = GenericRepository.FindOne<Y_NORM_PROFILE_DETAIL>(y => y.ID_PARAM == id);
            if (normCell != null || profileDetail != null)
            {
                temp = false;
            }
            return temp;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            _parametersCollection = GenericRepository.GetAllObservableCollection<Y_NORM_PARAMETERS>();
            parametersGridControl.ItemsSource = _parametersCollection;
            ((TableView)parametersGridControl.View).ShowingEditor += View_ShowingEditor;
            InitializeMenuButtons();
        }

        private void InitializeMenuButtons()
        {
            _btnParamAdd = UIHelper.FindChild<Button>(parametersGridControl, "btnAddParameter");
            _btnParamDelete = UIHelper.FindChild<Button>(parametersGridControl, "btnDeleteParameter");
            _btnSave = UIHelper.FindChild<Button>(parametersGridControl, "btnSave");
            _btnExit = UIHelper.FindChild<Button>(parametersGridControl, "btnExit");
            _btnParamAdd.Click += ButtonAddClick;
            _btnParamDelete.Click += BtnRemoveClick;
            _btnSave.Click += BtnOkClick;
            _btnExit.Click += BtnCloseClick;
        }

        private void BtnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void View_ShowingEditor(object sender, ShowingEditorEventArgs e)
        {
            if (!IsNotExistsParamInNormativeOrProfile(((Y_NORM_PARAMETERS) e.Row).ID))
            {
                e.Cancel = true;
            }
        }

        private void BtnRemoveClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var row = parametersGridControl.GetFocusedRow() as Y_NORM_PARAMETERS;
                if (row == null) throw new ArgumentException("Строка не выбрана!");
                if (IsNotExistsParamInNormativeOrProfile(row.ID))
                {
                    MessageBoxResult result = MessageBox.Show("Вы действительно желаете удалить профиль?",
                                                              "Удаление профиля",
                                                              MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (result == MessageBoxResult.Yes)
                    {
                        _parametersCollection.Remove(row);
                        GenericRepository.Delete(row);
                    }
                }
                else
                {
                    MessageBox.Show("Параметр используется", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                GenericRepository.UnitOfWork.BeginTransaction();
                GenericRepository.UnitOfWork.CommitTransaction();
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
            return _parametersCollection.Any(y => y.EntityState != EntityState.Unchanged) ||
                   GenericRepository.GetAll<Y_NORM_PARAMETERS>().Any(y => y.EntityState != EntityState.Unchanged);
        }

        private void WindowEquipTypeClosing(object sender, CancelEventArgs e)
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