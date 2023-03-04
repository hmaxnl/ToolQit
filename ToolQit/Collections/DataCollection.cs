using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using ToolQit.Converters;

namespace ToolQit.Collections
{
    public class DataCollection
    {
        public DataCollection(char separator = '.')
        {
            _separator = separator;
        }

        private DataCollection(DataCollection parent)
        {
            _separator = parent._separator;
            CollectionJsonOptions = parent.CollectionJsonOptions;
        }

        private readonly Dictionary<string, DataCollection> _collections = new Dictionary<string, DataCollection>();
        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();
        private readonly char _separator;

        public void Set(string key, string sValue) => _data[key] = sValue;
        public void Set(string key, long lValue) => _data[key] = lValue;
        public void Set(string key, double dValue) => _data[key] = dValue;
        public void Set(string key, bool bValue) => _data[key] = bValue;
        public void AddCollection(string key, DataCollection collection) => _collections.Add(key, collection);

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
        
        public ReadOnlyDictionary<string, DataCollection> DataCollections => new ReadOnlyDictionary<string, DataCollection>(_collections);
        public ReadOnlyDictionary<string, object> Data => new ReadOnlyDictionary<string, object>(_data);

        public JsonSerializerOptions CollectionJsonOptions = new JsonSerializerOptions()
            { Converters = { new DataCollectionJsonConverter() } };

        public string ToJson() => JsonSerializer.SerializeToNode(this, CollectionJsonOptions)?.ToString() ?? string.Empty;

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

    internal class KeyQueue
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