using System;

namespace NormManagementMVVM.Model
{
    public class ExceptionEventArgs : EventArgs
    {
        public Exception Exception { get; set; }
    }
}