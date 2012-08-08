using System;
using System.Data.Objects.DataClasses;
using System.Linq;

namespace NormManagementMVVM.Model
{
    public class IdGenerator
    {
        public static int GetId(EntityCollection<Y_NORM_NORMATIVE_HEAD> collection)
        {
            if (collection.Count == 0) return 1;
            return (Int32) collection.Max(y => y.ID) + 1;
        }

        public static int GetId(EntityCollection<Y_NORM_NORMATIVE_ROW> collection)
        {
            if (collection.Count == 0) return 1;
            return (Int32) collection.Max(y => y.ID_ROW) + 1;
        }

        public static int GetId(EntityCollection<Y_NORM_NORMATIVE_CELL> collection)
        {
            if (collection.Count == 0) return 1;
            return (Int32) collection.Max(y => y.ID_COLUMN) + 1;
        }
    }
}