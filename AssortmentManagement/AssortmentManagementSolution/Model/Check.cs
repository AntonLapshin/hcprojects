using AssortmentManagement.UserValues;

namespace AssortmentManagement.Model
{
    public class Check
    {
        public int N { get; set; }
        public string Desc { get; set; }
        public CheckStatuses Status { get; set; }
        public string ProcedureName { get; set; }
        public bool Selected { get; set; }
        public string TableName { get; set; }

        public override string ToString()
        {
            return Desc;
        }
    }
}