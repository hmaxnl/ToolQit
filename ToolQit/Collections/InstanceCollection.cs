using System;
using System.Collections.Generic;
using ToolQit.Extensions;

namespace ToolQit.Collections
{
    public class InstanceCollection
    {
        public void Register<TInstance>(string key, TInstance? instance = default)
        {
            if (key.IsNullEmptyWhiteSpace()) return;
            _instances[key] = new InstanceData() { Instance = instance, InstanceType = typeof(TInstance) };
        }
        public TInstance? Get<TInstance>(string key)
        {
            if (key.IsNullEmptyWhiteSpace() || !_instances.TryGetValue(key, out InstanceData iData)) return default;
            if (typeof(TInstance) != iData.InstanceType) return default;
            if (iData.Instance != null)
                return (TInstance)iData.Instance;
            iData.Instance = Activator.CreateInstance(iData.InstanceType);
            if (iData.Instance == null)
                return default;
            _instances[key] = iData;
            return (TInstance)iData.Instance;
        }
        public bool Remove(string key) => _instances.Remove(key);

        private readonly Dictionary<string, InstanceData> _instances = new Dictionary<string, InstanceData>();
    }

    internal struct InstanceData
    {
        public object? Instance;
        public Type InstanceType;
    }
}