using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ToolQit.Containers
{
    public class DataContainer : KeyContainerBase<DataContainer>
    {
        public DataContainer()
        { }
        public DataContainer(char separator) : base(separator)
        { }

        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();
        public ReadOnlyDictionary<string, object> Data => new ReadOnlyDictionary<string, object>(_data);

        public void Set(string key, string sValue, bool overridable = true)
        {
            if (ContainsKey(key) && !overridable)
                return;
            _data[key] = sValue;
        }

        public void Set(string key, long lValue, bool overridable = true)
        {
            if (ContainsKey(key) && !overridable)
                return;
            _data[key] = lValue;
        }

        public void Set(string key, double dValue, bool overridable = true)
        {
            if (ContainsKey(key) && !overridable)
                return;
            _data[key] = dValue;
        }

        public void Set(string key, bool bValue, bool overridable = true)
        {
            if (ContainsKey(key) && !overridable)
                return;
            _data[key] = bValue;
        }

        public bool ContainsKey(string key) => _data.ContainsKey(key);

        public string GetString(string key) => Convert.ToString(_data[key]);
        public long GetLong(string key) => Convert.ToInt64(_data[key]);
        public double GetDouble(string key) => Convert.ToDouble(_data[key]);
        public bool GetBool(string key) => Convert.ToBoolean(_data[key]);

        public bool Unset(string key) => _data.Remove(key);
        public void ClearData() => _data.Clear();
    }
}