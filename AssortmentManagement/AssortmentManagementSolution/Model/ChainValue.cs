using AssortmentManagement.UserValues;

namespace AssortmentManagement.Model
{
    public class ChainValue
    {
        public ValueStates State { get; set; }
        public object Value { get; set; }

        public ChainValue()
        {
            
        }

        public ChainValue(object value)
        {
            State = ValueStates.Valid;
            Value = value;
        }

        public override string ToString()
        {
            if (State == ValueStates.Various) return "?";
            return Value != null ? Value.ToString() : "";
        }
    }
}