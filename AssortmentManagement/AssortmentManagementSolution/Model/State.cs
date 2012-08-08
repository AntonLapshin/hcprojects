using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssortmentManagement.Model
{
    public class StateHistory
    {
        public List<StateRows> History { get; private set; }
        public int Index { get; set; }

        public StateHistory()
        {
            Index = 0;
            History = new List<StateRows>();
        }

        public void CreateNewState(StateRows state)
        {
            if (Index < History.Count)
                History.RemoveRange(Index, History.Count - Index);
            History.Add(state);
            Index++;
        }

        public StateRows GetPreviousState()
        {
            return Index == 0 ? null : History[--Index];
        }

        public StateRows GetNextState()
        {
            return Index == History.Count ? null : History[Index++];
        }
    }
    public class StateRows
    {
        public List<StateRow> Rows { get; set; }

        public StateRows()
        {
            Rows = new List<StateRow>();
        }
    }
    public class StateRow
    {
        public string Item { get; set; }
        public int Loc { get; set; }
        public List<FieldValue> FieldsValuesPrevious { get; set; }
        public List<FieldValue> FieldsValuesNext { get; set; }
    }
}
