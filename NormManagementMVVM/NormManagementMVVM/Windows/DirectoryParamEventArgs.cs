using System;
using NormManagementMVVM.Model;

namespace NormManagementMVVM.Windows
{
    public class DirectoryParamEventArgs : EventArgs
    {
        public int Id { get; set; }
        public Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result ParameterValues { get; set; }
    }
}