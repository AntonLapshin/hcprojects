using System.Collections.Generic;
using System.Collections.ObjectModel;
using NormManagementMVVM.Model;

namespace NormManagementMVVM.ViewModel.Messages
{
    public class MessageArgsParameter
    {
        public List<Y_NORM_PARAMETERS> Parameters { get; set; }
    }

    public class MessageArgsParameterCallback
    {
        public Y_NORM_PARAMETERS Parameter { get; set; }
    }

    public class MessageArgsParameterValues
    {
        public int Id { get; set; }
        public int IdParam { get; set; }
        public string Clause { get; set; }
        public ObservableCollection<ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result>> Values { get; set; }
    }

    public class MessageArgsParameterValuesCallback
    {
        public ObservableCollection<ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result>> GroupsRight { get; set; }
    }
}
