using System.Collections.Generic;
using ToolQit.Extensions;

namespace ToolQit.Collections
{
    public class InstanceCollection
    {
        public InstanceCollection()
        {
            
        }

        public void Register<TInstance>(string name, TInstance instance)
        {
            if (name.IsNullEmptyWhiteSpace()) return;
            _instances[name] = instance;
        }

        public TInstance? Get<TInstance>(string name)
        {
            if (name.IsNullEmptyWhiteSpace() || _instances.TryGetValue(name, out object? obj)) return default;
            if (obj?.GetType() == typeof(TInstance))
                return (TInstance?)obj;
            return default;
        }

        private readonly Dictionary<string, object?> _instances = new Dictionary<string, object?>();
    }
}