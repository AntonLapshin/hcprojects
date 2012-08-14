using System.Dynamic;
using Newtonsoft.Json.Linq;

namespace tfs2010sync
{
    public class DynamicJSON : DynamicObject
    {
        private JObject _data;

        public DynamicJSON(string data)
        {
            _data = JObject.Parse(data);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = _data[binder.Name];
            return true;
        }
    }
}
