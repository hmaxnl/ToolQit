using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ToolQit.Tools
{
    public class PropertyContainer : IDictionary<string, object?>
    {
        private readonly Dictionary<string, PropertyEntry> _entries = new Dictionary<string, PropertyEntry>();
        public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
        {
            Dictionary<string, object?> enumDict = _entries.ToDictionary(propEntry => propEntry.Key, propEntry => propEntry.Value.Data);
            return enumDict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(KeyValuePair<string, object?> item) => Add(item.Key, item.Value, true);
        public void Add(string key, object? value) => Add(key, value, true);
        public void Add(string key, object? value, bool serializable)
        {
            if (_entries.TryGetValue(key, out PropertyEntry propEntry))
            {
                propEntry.Data = value;
                _entries[key] = propEntry;
                return;
            }
            _entries.Add(key, new PropertyEntry() {Data = value, Serializable = serializable});
        }

        public void Clear() => _entries.Clear();

        public bool Contains(KeyValuePair<string, object?> item) => ContainsKey(item.Key);
        public bool ContainsKey(string key) => _entries.ContainsKey(key);

        public void CopyTo(KeyValuePair<string, object?>[] array, int arrayIndex)
        {
            if (arrayIndex > Count) return;
            KeyValuePair<string, object?>[] thisArr = this.ToArray();
            for (int i = 0; i < Count; i++)
            {
                if (arrayIndex + i > Count) return;
                array[i] = thisArr[arrayIndex + i];
            }
        }

        public bool Remove(KeyValuePair<string, object?> item) => Remove(item.Key);
        public bool Remove(string key) => _entries.ContainsKey(key) && _entries.Remove(key);

        public int Count => _entries.Count;
        public bool IsReadOnly => false;
        
        public bool TryGetValue(string key, out object? value)
        {
            if (_entries.TryGetValue(key, out PropertyEntry propEntry))
            {
                value = propEntry.Data;
                return true;
            }
            value = null;
            return false;
        }

        public object? this[string key]
        {
            get => _entries.TryGetValue(key, out PropertyEntry propEntry) ? propEntry.Data : null;
            set => Add(key, value);
        }

        public object? this[string key, bool serializable]
        {
            get => this[key];
            set => Add(key, value, serializable);
        }

        public ICollection<string> Keys => _entries.Keys;
        public ICollection<object?> Values {
            get
            {
                List<object?> objList = new List<object?>();
                _entries.Values.ToList().ForEach(x => objList.Add(x.Data));
                return objList;
            }
        }
    }

    public struct PropertyEntry
    {
        public object? Data;
        public bool Serializable;
    }
}