using System.Collections.Generic;

namespace ToolQit.Collections
{
    public class DataCollection
    {
        private readonly Dictionary<string, DataCollection> _data = new Dictionary<string, DataCollection>();
        private readonly Dictionary<string, string> _contents = new Dictionary<string, string>();

        public void SetString(string key, string value) => _contents[key] = value;

        public string GetString(string key) => !_contents.ContainsKey(key) ? string.Empty : _contents[key];

        public DataCollection this[string key]
        {
            get
            {
                if (!_data.ContainsKey(key))
                    _data.Add(key, new DataCollection());
                return _data[key];
            }
        }
    }

    class CollectionFinder
    {
        public CollectionFinder(string path, char divider = '.')
        {
            _path = new Queue<string>(path.Split(divider));
        }
        public string Next() => _path.Dequeue();

        private readonly Queue<string> _path;
    }
}