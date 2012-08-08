using System.Collections.Generic;
using System.Linq;

namespace NormManagementMVVM.Controls
{
    public static class Controllers
    {
        static Controllers()
        {
            CellControllers = new Dictionary<int?, GroupCellController>();
        }

        public static Dictionary<int?, GroupCellController> CellControllers { get; set; }

        public static int? GetKeyByValue(GroupCellController value)
        {
            foreach (
                var recordOfDictionary in
                    CellControllers.Where(recordOfDictionary => recordOfDictionary.Value.Equals(value)))
            {
                return recordOfDictionary.Key;
            }
            return null;
        }

        public static void Remove(GroupCellController controller)
        {
            int? tempKey = GetKeyByValue(controller);
            if (tempKey == null) return;
            var intTempKey = (int) tempKey;
            CellControllers.Remove(intTempKey);
        }

        public static int? NewKey()
        {
            return CellControllers.Keys.Count == 0
                       ? 1
                       : CellControllers.Keys.Max() + 1;
        }
    }
}