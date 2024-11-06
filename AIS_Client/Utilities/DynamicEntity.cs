using System.Collections.Generic;
using System.Dynamic;

namespace AIS_Client.Utilities
{
    public class DynamicEntity: DynamicObject
    {
        private readonly Dictionary<string, object> _fields;

        public DynamicEntity(Dictionary<string, object> fields)
        {
            _fields = fields;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_fields.TryGetValue(binder.Name, out result))
            {
                return true;
            }
            return base.TryGetMember(binder, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (_fields.ContainsKey(binder.Name))
            {
                _fields[binder.Name] = value;
                return true;
            }
            return base.TrySetMember(binder, value);
        }

        public Dictionary<string, object> GetFields()
        {
            return _fields;
        }
    }
}
