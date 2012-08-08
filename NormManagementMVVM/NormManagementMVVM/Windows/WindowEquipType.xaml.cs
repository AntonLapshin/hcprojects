using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using NormManagementMVVM.Model;
using SharedComponents.Connection;
using SharedComponents.UI;

namespace NormManagementMVVM.Windows
{
    /// <summary>
    /// Логика взаимодействия для WindowEquipType.xaml
    /// </summary>
    public partial class WindowEquipType : Window, ISaved
    {
        private ObservableCollection<Y_NORM_EQUIP_TYPE> _equipTypeCollection;
        private Button _btnAddEquipType;
        private Button _btnSave;
        private Button _btnExit;

        public WindowEquipType()
        {
            InitializeComponent();
                     
        }

        void WindowEquipType_Loaded(object sender, RoutedEventArgs e)
        {
            _equipTypeCollection = GenericRepository.GetAllObservableCollection<Y_NORM_EQUIP_TYPE>();
            equipTypeControl.ItemsSource = _equipTypeCollection;
            InitializeMenuButtons();
        }
        private void InitializeMenuButtons()
        {
            _btnAddEquipType = UIHelper.FindChild<Button>(equipTypeControl, "btnAddEquipType");
            _btnSave = UIHelper.FindChild<Button>(equipTypeControl, "btnSave");
            _btnExit = UIHelper.FindChild<Button>(equipTypeControl, "btnExit");
            _btnAddEquipType.Click += ButtonAddClick;
            _btnExit.Click += BtnCloseClick;
            _btnSave.Click += BtnOkClick;
        }
        #region ISaved Members

        public bool IsSaved { get; set; }

        #endregion
        private void BtnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonAddClick(object sender, RoutedEventArgs e)
        {
            var equipType = new Y_NORM_EQUIP_TYPE
                                {
                                    ID = _equipTypeCollection.Max(y => y.ID) + 1,
                                    CREATE_DATETIME = DateTime.Now,
                                    CREATE_ID = User.Name.ToUpper()
                                };
            _equipTypeCollection.Add(equipType);
            GenericRepository.Add(equipType);
            equipTypeControl.View.FocusedRow = equipType;
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
            return _equipTypeCollection.Any(y => y.EntityState != EntityState.Unchanged) ||
                   GenericRepository.GetAll<Y_NORM_EQUIP_TYPE>().Any(y => y.EntityState != EntityState.Unchanged);
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