using System.Collections.Generic;

namespace AssortmentManagement.Model
{
    public class FilterValues
    {
        public string Field { get; set; }
        public List<object> Values { get; set; }
        public bool ShowBlanks { get; set; }

        public FilterValues()
        {
            Values = new List<object>();
        }

        public override string ToString()
        {
            return Field + " (" + Values.Count + ")" + (ShowBlanks ? "(show blanks)" : "");
        }
    }
}