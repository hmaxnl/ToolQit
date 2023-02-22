using System;
using System.Collections.Generic;

namespace ToolQit.Collections
{
    public class DataCollection
    {
        public DataCollection(char dividerChar = '.')
        {
            _dividerChar = dividerChar;
        }
        private readonly Dictionary<string, DataCollection> _data = new Dictionary<string, DataCollection>();
        private readonly Dictionary<string, string> _contents = new Dictionary<string, string>();
        private readonly char _dividerChar;

        public void SetString(string key, string value) => _contents[key] = value;

        public string GetString(string key) => !_contents.ContainsKey(key) ? string.Empty : _contents[key];

        public DataCollection this[string key]
        {
            get
            {
                if (key.Contains(_dividerChar))
                {
                    AddFromQueue(new KeyQueue(key, _dividerChar));
                    //TODO: Get the data!
                }

                CheckAddKey(key);
                return _data[key];
            }
        }

        private void AddFromQueue(KeyQueue queue)
        {
            if (queue.IsEmpty) return;
            string queueKey = queue.Next();
            CheckAddKey(queueKey);
            _data[queueKey].AddFromQueue(queue);
        }

        private void CheckAddKey(string key)
        {
            if (!_data.ContainsKey(key))
                _data.Add(key, new DataCollection(_dividerChar));
        }
    }

    class KeyQueue
    {
        public KeyQueue(string key, char divider = '.')
        {
            _key = new Queue<string>(key.Split(divider, StringSplitOptions.RemoveEmptyEntries));
        }
        public string Next() => _key.Dequeue();
        public bool IsEmpty => _key.Count <= 0;

        private readonly Queue<string> _key;
    }
}