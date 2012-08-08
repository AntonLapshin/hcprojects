
namespace NormManagementMVVM.ViewModel
{

    public class ViewModelLocator
    {
        private static MainViewModel _main;
        private static ParametersViewModel _parameters;
        private static ParameterValuesViewModel _parameterValues;

        public ParametersViewModel Parameters
        {
            get
            {
                return _parameters;
            }
        }
        public MainViewModel Main
        {
            get
            {
                return _main;
            }
        }
        public ParameterValuesViewModel ParameterValues
        {
            get
            {
                return _parameterValues;
            }
        }        

        public ViewModelLocator()
        {
            _main = new MainViewModel();
            _parameters = new ParametersViewModel();
            _parameterValues = new ParameterValuesViewModel();
        }
    }
}