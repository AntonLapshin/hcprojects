namespace AssortmentManagement.Model
{
    public class FieldValue
    {
        public string Field { get; set; }
        public object Value { get; set; }
        public bool ShowBlanks { get; set; }

        public override string ToString()
        {
            return Field + " = " + Value + (ShowBlanks ? "(show blanks)" : "");
        }
    }
}