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
    /// Логика взаимодействия для WindowProfile.xaml
    /// </summary>
    public partial class WindowProfile : Window, ISaved
    {
        private readonly ObservableCollection<Y_NORM_PROFILE_DETAIL> _addDetailCollection;
        private readonly ObservableCollection<Y_NORM_PROFILE_DETAIL> _removeDetailCollection;
        private ObservableCollection<Y_NORM_PROFILE_DETAIL> _detailCollection;
        private ObservableCollection<Y_NORM_PROFILE_HEAD> _headCollection;
        private Button _btnAddHead;
        private Button _btnDeleteHead;
        private Button _btnSave;
        private Button _btnExit;
        private Button _btnAddDetail;
        private Button _btnDeleteDetail;

        public WindowProfile()
        {
            InitializeComponent();
            _headCollection = new ObservableCollection<Y_NORM_PROFILE_HEAD>();
            _detailCollection = new ObservableCollection<Y_NORM_PROFILE_DETAIL>();
            _addDetailCollection = new ObservableCollection<Y_NORM_PROFILE_DETAIL>();
            _removeDetailCollection = new ObservableCollection<Y_NORM_PROFILE_DETAIL>();
            //profileDetailgridControl.View.IsEnabled = false;
        }

        #region ISaved Members

        public bool IsSaved { get; set; }

        #endregion



        private void View_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            RowChange();
        }

        private void View_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var view = profileHeadGridControl.View as TableView;
            TableViewHitInfo hi = view.CalcHitInfo(e.OriginalSource as DependencyObject);
            if (hi.InRow && hi.Column != null)
            {
                if (hi.Column.FieldName.Equals("Y_NORM_EQUIP_TYPE.DESCRIPTION"))
                {
                    var winParam = new WindowDirectoryParameters(23, false);
                    winParam.DirectoryParamSelected += winParam_DirectoryParamSelected;
                    winParam.ShowDialog();
                }
            }
        }

        private void RowChange()
        {
            var row = profileHeadGridControl.GetFocusedRow() as Y_NORM_PROFILE_HEAD;
            if (row == null)
            {
                _detailCollection = new ObservableCollection<Y_NORM_PROFILE_DETAIL>();
                profileDetailgridControl.ItemsSource = _detailCollection;
            }
            else
            {
                _detailCollection =
                    GenericRepository.GetQuery<Y_NORM_PROFILE_DETAIL>(y => y.ID == row.ID).ToObservableCollection();
                foreach (Y_NORM_PROFILE_DETAIL addedRow in _addDetailCollection.Where(y => y.ID == row.ID))
                {
                    _detailCollection.Add(addedRow);
                }
                foreach (Y_NORM_PROFILE_DETAIL removeRow in _removeDetailCollection.Where(y => y.ID == row.ID))
                {
                    _detailCollection.Remove(removeRow);
                }
                profileDetailgridControl.ItemsSource = _detailCollection;
                IsEnableDetail(row.ID);
            }
        }

        private void winParam_DirectoryParamSelected(object sender, DirectoryParamEventArgs e)
        {
            var row = profileHeadGridControl.GetFocusedRow() as Y_NORM_PROFILE_HEAD;
            row.ID_EQUIP = long.Parse(e.ParameterValues.VALUE);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _headCollection = GenericRepository.GetAllObservableCollection<Y_NORM_PROFILE_HEAD>();
            _headCollection.Sort(y => y.ID);
            profileHeadGridControl.ItemsSource = _headCollection;
            profileHeadGridControl.View.PreviewMouseDoubleClick += View_PreviewMouseDoubleClick;
            profileDetailgridControl.View.PreviewMouseDoubleClick += View_DetailPreviewMouseDoubleClick;
            profileHeadGridControl.View.FocusedRowChanged += View_FocusedRowChanged;
            InitializeMenuButtons();
        }

        private void InitializeMenuButtons()
        {
            _btnAddHead = UIHelper.FindChild<Button>(profileHeadGridControl, "btnAddHead");
            _btnDeleteHead = UIHelper.FindChild<Button>(profileHeadGridControl, "btnDeleteHead");
            _btnSave = UIHelper.FindChild<Button>(profileHeadGridControl, "btnSave");
            _btnExit = UIHelper.FindChild<Button>(profileHeadGridControl, "btnExit");
            _btnAddDetail = UIHelper.FindChild<Button>(profileDetailgridControl, "btnAddDetail");
            _btnDeleteDetail = UIHelper.FindChild<Button>(profileDetailgridControl, "btnDeleteDetail");
            _btnAddHead.Click += btnHeadAdd_Click;
            _btnDeleteHead.Click += btnHeadRemove_Click;
            _btnSave.Click += btnHeadOk_Click;
            _btnExit.Click += btnExitClick;
            _btnAddDetail.Click += btnDetailAdd_Click;
            _btnDeleteDetail.Click += btnDetailRemove_Click;
        }


        private bool IsNotExistsProfileInNormative(long id)
        {
            bool temp = true;
            var profile = GenericRepository.FindOne<Y_NORM_NORMATIVE_HEAD>(y => y.Y_NORM_PROFILE_HEAD.ID == id);
            if (profile != null)
            {
                temp = profile.Y_NORM_NORMATIVE_ROW.Count == 0;
            }
            return temp;
        }
        private void btnExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void btnHeadAdd_Click(object sender, RoutedEventArgs e)
        {
            var profileHead = new Y_NORM_PROFILE_HEAD
                                  {
                                      ID = _headCollection.Max(y => y.ID) + 1,
                                      CREATE_DATETIME = DateTime.Now,
                                      CREATE_ID = User.Name.ToUpper()
                                  };
            _headCollection.Add(profileHead);
            GenericRepository.Add(profileHead);
            //_addHeadCollection.Add(profileHead);
            profileHeadGridControl.View.FocusedRow = profileHead;
        }

        private void btnHeadRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var row = profileHeadGridControl.GetFocusedRow() as Y_NORM_PROFILE_HEAD;
                if (row == null) throw new ArgumentException("Строка не выбрана!");
                if (IsNotExistsProfileInNormative(row.ID))
                {
                    MessageBoxResult result = MessageBox.Show("Вы действительно желаете удалить профиль?",
                                                              "Удаление профиля",
                                                              MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (result == MessageBoxResult.Yes)
                    {
                        _headCollection.Remove(row);

                        GenericRepository.Delete(row);
                    }
                    //_removeHeadCollection.Add(row);
                }
                else
                {
                    MessageBox.Show("Этот профиль задействован в нормативе. Удалите норматив перед удалением профиля.",
                                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDetailAdd_Click(object sender, RoutedEventArgs e)
        {
            var profileDetail = new Y_NORM_PROFILE_DETAIL { ID = ((Y_NORM_PROFILE_HEAD)profileHeadGridControl.GetFocusedRow()).ID };
            _detailCollection.Add(profileDetail);
            //GenericRepository.Add(profileDetail);
            _addDetailCollection.Add(profileDetail);
            profileDetailgridControl.View.FocusedRow = profileDetail;
        }

        private void IsEnableDetail(long headId)
        {
            bool enabled = IsNotExistsProfileInNormative(headId);
            _btnAddDetail.IsEnabled = enabled;
            _btnDeleteDetail.IsEnabled = enabled;
        }

        private void btnDetailRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var row = profileDetailgridControl.GetFocusedRow() as Y_NORM_PROFILE_DETAIL;
                if (row == null) throw new ArgumentException("Строка не выбрана!");
                _detailCollection.Remove(row);
                _removeDetailCollection.Add(row);
                if (row.EntityKey == null) _addDetailCollection.Remove(row);
                //GenericRepository.Delete(row);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void View_DetailPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var view = profileDetailgridControl.View as TableView;
            TableViewHitInfo hi = view.CalcHitInfo(e.OriginalSource as DependencyObject);
            if (hi.InRow && hi.Column != null)
            {
                var row = profileDetailgridControl.GetRow(hi.RowHandle) as Y_NORM_PROFILE_DETAIL;
                if (IsNotExistsProfileInNormative(row.ID))
                {
                    if (hi.Column.FieldName.Equals("ID_PARAM") ||
                        hi.Column.FieldName.Equals("Y_NORM_PARAMETERS.DESC_RU"))
                    {
                        if (_addDetailCollection.Contains(row))
                        {
                            var winParam = new WindowEditParameters();
                            winParam.ParameterSelected += winParam_ParameterSelected;
                            winParam.ShowDialog();
                        }
                    }
                    if ((hi.Column.FieldName.Equals("VALUE") || hi.Column.FieldName.Equals("VALUE_DESC")) &&
                        row.ID_PARAM != 0)
                    {
                        var winParamValues = new WindowDirectoryParameters(Convert.ToInt32(row.ID_PARAM), true);
                        winParamValues.DirectoryParamSelected += winParamValues_DirectoryParamSelected;
                        winParamValues.ShowDialog();
                    }
                }
            }
        }

        private void winParamValues_DirectoryParamSelected(object sender, DirectoryParamEventArgs e)
        {
            var row = profileDetailgridControl.GetFocusedRow() as Y_NORM_PROFILE_DETAIL;
            row.VALUE = e.ParameterValues.VALUE;
        }

        private void winParam_ParameterSelected(object sender, CellParamEventArgs e)
        {
            try
            {
                if (_detailCollection.Count(y => y.ID_PARAM == e.Parameter.ID) != 0)
                {
                    throw new ArgumentException("Такой параметр уже участвует в профиле!");
                }
                var row = profileDetailgridControl.GetFocusedRow() as Y_NORM_PROFILE_DETAIL;
                row.ID_PARAM = e.Parameter.ID;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnHeadOk_Click(object sender, RoutedEventArgs e)
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
                foreach (Y_NORM_PROFILE_DETAIL profileDetail in _addDetailCollection)
                {
                    GenericRepository.Add(profileDetail);
                }
                foreach (Y_NORM_PROFILE_DETAIL profileDetail in _removeDetailCollection)
                {
                    GenericRepository.Delete(profileDetail);
                }
                GenericRepository.UnitOfWork.BeginTransaction();
                GenericRepository.UnitOfWork.CommitTransaction();
                _addDetailCollection.Clear();
                _removeDetailCollection.Clear();
                profileHeadGridControl.RefreshData();
                profileDetailgridControl.RefreshData();
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

        private void WindowEquipStore_Closing(object sender, CancelEventArgs e)
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

        private bool GetChanges()
        {
            return GenericRepository.GetAll<Y_NORM_PROFILE_HEAD>().Any(y => y.EntityState != EntityState.Unchanged) ||
                   _headCollection.Any(y => y.EntityState != EntityState.Unchanged) ||
                   _detailCollection.Any(y => y.EntityState != EntityState.Unchanged) ||
                   GenericRepository.GetAll<Y_NORM_PROFILE_DETAIL>().Any(y => y.EntityState != EntityState.Unchanged);
        }
    }
}