using System;
using System.Collections.Generic;
using System.Text.Json;
using ToolQit.Converters;

namespace ToolQit.Collections
{
    //TODO: Implement serializer -> JSON & Text
    public class DataCollection
    {
        public DataCollection(char separator = '.')
        {
            _separator = separator;
        }

        private DataCollection(DataCollection parent)
        {
            _parent = parent;
            _separator = _parent._separator;
        }

        private readonly DataCollection? _parent = null;
        internal readonly Dictionary<string, DataCollection> _collections = new Dictionary<string, DataCollection>();
        internal readonly Dictionary<string, object> _data = new Dictionary<string, object>();
        private readonly char _separator;

        public void Set(string key, string sValue) => _data[key] = sValue;
        public void Set(string key, int iValue) => _data[key] = iValue;
        public void Set(string key, double dValue) => _data[key] = dValue;
        public void Set(string key, bool bValue) => _data[key] = bValue;
        
        public DataCollection this[string key]
        {
            get
            {
                if (key.Contains(_separator))
                    return AddFromQueue(new KeyQueue(key, _separator));

                CheckAddKey(key);
                return _collections[key];
            }
        }

        public string ToJson() => JsonSerializer.SerializeToNode(this, new JsonSerializerOptions() { Converters = { new DataCollectionJsonConverter() }})?.ToString() ?? String.Empty;

        private DataCollection AddFromQueue(KeyQueue queue)
        {
            if (queue.IsEmpty) return this;
            string queueKey = queue.Next();
            CheckAddKey(queueKey);
            return _collections[queueKey].AddFromQueue(queue);
        }

        private void CheckAddKey(string key)
        {
            if (!_collections.ContainsKey(key))
                _collections.Add(key, new DataCollection(this));
        }
    }

    class KeyQueue
    {
        public KeyQueue(string key, char divider)
        {
            _key = new Queue<string>(key.Split(divider, StringSplitOptions.RemoveEmptyEntries));
        }
        public string Next() => _key.Dequeue();
        public bool IsEmpty => _key.Count <= 0;

        private readonly Queue<string> _key;
    }
}