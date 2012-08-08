namespace AssortmentManagement.Model
{
    public class Column
    {
        public string Name { get; set; }
        public string Desc { get; set; }

        public override string ToString()
        {
            return Name + " " + Desc;
        }
    }
}