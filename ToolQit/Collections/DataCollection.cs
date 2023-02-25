using System;
using System.Collections.Generic;
using System.Globalization;

namespace ToolQit.Collections
{
    //TODO: Implement serializer -> JSON & Text
    public class DataCollection
    {
        public DataCollection(char dividerChar = '.')
        {
            _dividerChar = dividerChar;
        }
        private readonly Dictionary<string, DataCollection> _collections = new Dictionary<string, DataCollection>();
        private readonly Dictionary<int, string> _dataDictionary = new Dictionary<int, string>();
        private readonly char _dividerChar;

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
                if (key.Contains(_dividerChar))
                    return AddFromQueue(new KeyQueue(key, _dividerChar));

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
                _collections.Add(key, new DataCollection(_dividerChar));
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