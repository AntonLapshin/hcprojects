using System;

namespace NormManagementMVVM.Model
{
    public class CellParamEventArgs : EventArgs
    {
        public Y_NORM_PARAMETERS Parameter { get; set; }
    }
}