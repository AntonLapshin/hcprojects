using System;
using System.Collections.Generic;
using AssortmentManagement.UserValues;

namespace AssortmentManagement.Model
{
    public class ItemsAddEventArgs : EventArgs
    {
        public string Condition { get; set; }
        public List<string> ListCondition { get; set; }
    }
    public class ValuesEventArgs : EventArgs
    {
        public List<string> Values { get; set; }
        public bool Result { get; set; }
        public string Error { get; set; }
    }
    public class SupplierEventArgs : EventArgs
    {
        public int Supplier { get; set; }
        public string Name { get; set; }
    }
    public class CellInputDataEventArgs : EventArgs
    {
        public InputDataTypes Type { get; set; }
        public List<FieldValue> SetValues { get; set; }
        public List<FieldValue> ConditionValues { get; set; }
        public List<FilterValues> Filters { get; set; }

        public CellInputDataEventArgs()
        {
            SetValues = new List<FieldValue>();
            ConditionValues = new List<FieldValue>();
        }
    }
    public class CellActionEventArgs : EventArgs
    {
        public FormActions ActionType { get; set; }
        public List<FieldValue> ConditionValues { get; set; }
        public List<FilterValues> Filters { get; set; }
    }
}
