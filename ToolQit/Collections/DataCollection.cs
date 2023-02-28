using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

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
        private readonly Dictionary<string, DataCollection> _collections = new Dictionary<string, DataCollection>();
        private readonly Dictionary<int, string> _dataDictionary = new Dictionary<int, string>();
        private readonly char _separator;

        public void SetString(string sValue, int index = 0) => _dataDictionary[index] = sValue;
        public void SetInt(int iValue, int index = 0) => SetString(Convert.ToString(iValue), index);
        public void SetReal(double dValue, int index = 0) => SetString(Convert.ToString(dValue, CultureInfo.CurrentCulture), index);

        public string GetString(int index = 0) => _dataDictionary.Count < index ? String.Empty : _dataDictionary[index];
        public int GetInt(int index = 0) => Convert.ToInt32(GetString(index));
        public double GetReal(int index = 0) => Convert.ToDouble(GetString(index));

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

        public override string ToString()
        {
            StringBuilder data = new StringBuilder();
            
            return base.ToString();
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