using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NormManagementMVVM.Model;
using SharedComponents.Connection;

namespace NormManagementMVVM.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand<Y_NORM_PROFILE_HEAD> CommandChangeProfile { get; private set; }
        public IOrderedEnumerable<Y_NORM_PROFILE_HEAD> Profiles { get; private set; }
        public HeadViewModel Head { get; private set; }

        public Visibility Load { get; private set; }

        public Visibility HeadVisibility
        {
            get { return Head == null ? Visibility.Hidden : Visibility.Visible; }
        }

        public void LoadShow()
        {
            Load = Visibility.Visible;
            RaisePropertyChanged("Load");
        }

        public void LoadHide()
        {
            Load = Visibility.Hidden;
            RaisePropertyChanged("Load");
        }

        public MainViewModel()
        {
            LoadShow();
            CommandChangeProfile = new RelayCommand<Y_NORM_PROFILE_HEAD>(CommandChangeProfileRelease);
            User.Name = "RMSPRD";
            User.Password = "golive104";
            RMSConnection.Current = RMSConnection.RMSTSTN;
            GenericRepository.InitializeServer();
            GetProfiles();
            LoadHide();
        }

        private void GetProfiles()
        {
            Profiles = GenericRepository.GetAll<Y_NORM_PROFILE_HEAD>().Where(y => y.Y_NORM_PROFILE_DETAIL.Count != 0).
                OrderBy(y => y.ToString());
        }

        private void CommandChangeProfileRelease(Y_NORM_PROFILE_HEAD profile)
        {
            Parallel.Invoke(() => LoadShow());
            LoadShow();
            Head = new HeadViewModel(profile.Y_NORM_NORMATIVE_HEAD.FirstOrDefault());
            RaisePropertyChanged("Head");
            RaisePropertyChanged("HeadVisibility");
            Parallel.Invoke(() => LoadHide());
        }
    }
}