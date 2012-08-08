using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NormManagementMVVM.Model
{
    public class ParamValuesEventArgs : EventArgs
    {
        public List<ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result>> ParamValues { get; set; }
    }
}