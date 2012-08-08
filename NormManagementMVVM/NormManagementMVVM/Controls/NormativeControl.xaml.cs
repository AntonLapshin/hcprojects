using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using NormManagementMVVM.Model;
using SharedComponents.Connection;

namespace NormManagementMVVM.Controls
{
    /// <summary>
    /// Логика взаимодействия для NormativeControl.xaml
    /// </summary>
    public partial class NormativeControl
    {

        private Y_NORM_NORMATIVE_HEAD _norm;

        public NormativeControl()
        {
            try
            {
                //Login.Name = "RMSPRD";
                //Login.Password = "golive104";
                InitializeComponent();
                DataContextChanged += NormativeControlDataContextChanged;
                IOrderedEnumerable<Y_NORM_PROFILE_HEAD> profiles =
                    GenericRepository.GetAll<Y_NORM_PROFILE_HEAD>().Where(y => y.Y_NORM_PROFILE_DETAIL.Count != 0).
                        OrderBy(y => y.ToString());
                cmbProfile.ItemsSource = profiles;
            }
            catch (Exception ex)
            {
                ExceptionRoute(ex);
                //MessageBox.Show(ex.Message, "Ошибка загрузки", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool IsSaved { get; set; }
        public event EventHandler<ExceptionEventArgs> ExceptionEvent;

        public void ExceptionRoute(Exception exception)
        {
            ExceptionEvent(this, new ExceptionEventArgs { Exception = exception });
        }

        private void NormativeControlDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var normativeDataContext = e.NewValue as Y_NORM_NORMATIVE_HEAD;
            SetProfileContent(normativeDataContext);
            normPanel.Children.Clear();
            if (normativeDataContext.Y_NORM_NORMATIVE_ROW.Count == 0)
            {
                CreateOneNewRow(normativeDataContext);
            }
            else
            {
                CreateRowControls(normativeDataContext);
            }
            foreach (GroupCellController controller in Controllers.CellControllers.Values)
            {
                controller.CheckController();
            }
        }

        private void CreateRowControls(Y_NORM_NORMATIVE_HEAD normativeDataContext)
        {
            foreach (
                var row in
                    normativeDataContext.Y_NORM_NORMATIVE_ROW.OrderBy(y => y.SEQ_NUM))
            {
                var rowControl = new RowControl { DataContext = row };
                normPanel.Children.Add(rowControl);
            }
        }

        private void CreateOneNewRow(Y_NORM_NORMATIVE_HEAD normativeDataContext)
        {
            var id = IdGenerator.GetId(normativeDataContext.Y_NORM_NORMATIVE_ROW);
            var row = new Y_NORM_NORMATIVE_ROW { ID_ROW = id, SEQ_NUM = id, MAX_COLUMN = 1, SKU = 0, DELTA = 0 };
            normativeDataContext.Y_NORM_NORMATIVE_ROW.Add(row);
            var rowControl = new RowControl { DataContext = row };
            normPanel.Children.Add(rowControl);
        }

        private void SetProfileContent(Y_NORM_NORMATIVE_HEAD norm)
        {
            profileGrpBox.Header = "Профиль: " + norm.Y_NORM_PROFILE_HEAD;
            paramProfileLabel.Content = "Параметры: ";
            foreach (Y_NORM_PROFILE_DETAIL profileParam in norm.Y_NORM_PROFILE_HEAD.Y_NORM_PROFILE_DETAIL)
            {
                SetProfileParamInHead(profileParam);
            }
        }

        private void SetProfileParamInHead(Y_NORM_PROFILE_DETAIL profileParam)
        {
            paramProfileLabel.Content += profileParam.Y_NORM_PARAMETERS.DESC_RU + ": " +
                                         string.Join(",",
                                                     GenericRepository.GetValues((int)profileParam.ID_PARAM,
                                                                                 profileParam.VALUE).Select(
                                                                                     y =>
                                                                                     y.VALUE + " (" + y.NAME + ")")) +
                                         "; ";
        }

        private void ButtonSaveClick(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private bool IsNormativeDeleted()
        {
            var head = DataContext as Y_NORM_NORMATIVE_HEAD;
            if (head.Y_NORM_NORMATIVE_ROW.FirstOrDefault(y => y.MAX_COLUMN == 0) != null)
            {
                head.Y_NORM_NORMATIVE_ROW.Clear();
                return true;
            }
            return false;
        }

        private void Save()
        {
            if (DataContext == null)
            {
                MessageBox.Show("Профиль не выбран!", "Профиль отсутствует", MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }
            if (IsNormativeDeleted())
            {
                try
                {
                    var result = MessageBox.Show("Вы действительно желаете удалить разбиение?", "Удаляем?",
                                                 MessageBoxButton.YesNo, MessageBoxImage.Information);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:

                            GenericRepository.UnitOfWork.BeginTransaction();

                            GenericRepository.UnitOfWork.CommitTransaction();
                            IsSaved = true;
                            break;

                        case MessageBoxResult.No:
                            IsSaved = false;
                            break;
                    }
                }
                catch (Exception e)
                {
                    ExceptionRoute(e);
                }
            }
            else
            {
                try
                {
                    GenericRepository.UnitOfWork.BeginTransaction();

                    GenericRepository.UnitOfWork.CommitTransaction();
                    IsSaved = true;
                }
                catch (Exception e)
                {
                    ExceptionRoute(e);
                }
            }
        }

        private void CmbProfileSelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                Controllers.CellControllers.Clear();

                _norm =
                    GenericRepository.FindOne<Y_NORM_NORMATIVE_HEAD>(
                        x => x.ID == ((Y_NORM_PROFILE_HEAD)cmbProfile.SelectedItem).ID) ??
                    new Y_NORM_NORMATIVE_HEAD
                        {
                            ID = ((Y_NORM_PROFILE_HEAD)cmbProfile.SelectedItem).ID,
                            Y_NORM_PROFILE_HEAD = (Y_NORM_PROFILE_HEAD)cmbProfile.SelectedItem,
                            CREATE_DATETIME = DateTime.Now,
                            LAST_UPDATE_ID = User.Name.ToUpper()
                        };
                DataContext = _norm;
            }
            catch (Exception ex)
            {
                ExceptionRoute(ex);
            }
        }

        public bool isChanged()
        {
            return GenericRepository.GetChanges();
        }

        public bool Saved(MessageBoxResult result)
        {
            bool saved = true;
            if (_norm == null) return saved;


            //try
            //{
            //var result = MessageBox.Show("Желаете ли вы сохранить текущие изменения?", "Сохранимся?",
            //MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Save();
                    break;
                case MessageBoxResult.No:
                    GenericRepository.RollbackContext();
                    break;
                case MessageBoxResult.Cancel:
                    saved = false;
                    break;
            }
            //}
            //catch (ArgumentException e)
            //{
            //ExceptionRoute(e);
            //throw;
            //}
            return saved;
        }

        private void cmbProfile_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (isChanged())
                {
                    Saved(MessageBox.Show("Желаете ли вы сохранить текущие изменения?", "Сохранимся?",
                                          MessageBoxButton.YesNoCancel, MessageBoxImage.Information));
                }
            }
            catch (Exception ex)
            {
                ExceptionRoute(ex);
            }
        }
    }
}