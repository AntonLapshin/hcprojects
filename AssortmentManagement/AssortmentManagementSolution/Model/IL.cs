using System;

namespace AssortmentManagement.Model
{
    public class IL : IComparable
    {
        public string Item { get; set; }
        public int Loc { get; set; }

        public override bool Equals(object obj)
        {
            return Item.Equals(((IL)obj).Item) && Loc.Equals(((IL)obj).Loc);
        }

        public int CompareTo(object obj)
        {
            return
                (Convert.ToInt64(Item + Loc)).CompareTo(
                    Convert.ToInt64(((IL)obj).Item + ((IL)obj).Loc));
        }

        public override string ToString()
        {
            return Item + " " + Loc;
        }

    }
}