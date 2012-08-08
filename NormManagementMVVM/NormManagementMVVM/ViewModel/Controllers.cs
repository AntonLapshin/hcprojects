using System.Collections.Generic;
using System.Linq;
using NormManagementMVVM.Model;

namespace NormManagementMVVM.ViewModel
{
    public class Controllers : Dictionary<int?,Controller>
    {
        public int? GetKeyByValue(Controller value)
        {
            foreach (
                var recordOfDictionary in
                    this.Where(recordOfDictionary => recordOfDictionary.Value.Equals(value)))
            {
                return recordOfDictionary.Key;
            }
            return null;
        }

        public void Remove(Controller controller)
        {
            int? tempKey = GetKeyByValue(controller);
            if (tempKey == null) return;
            var intTempKey = (int)tempKey;
            Remove(intTempKey);
        }

        public int? NewKey()
        {
            return Keys.Count == 0
                       ? 1
                       : Keys.Max() + 1;
        }
    }
}
