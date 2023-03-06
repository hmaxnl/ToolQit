using System;
using System.Collections.Generic;

namespace ToolQit.Containers
{
    public class DataContainer : KeyContainerBase<DataContainer>
    {
        public DataContainer()
        { }
        public DataContainer(char separator) : base(separator)
        { }
        
        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

        public void Set(string key, string sValue) => _data[key] = sValue;
        public void Set(string key, long lValue) => _data[key] = lValue;
        public void Set(string key, double dValue) => _data[key] = dValue;
        public void Set(string key, bool bValue) => _data[key] = bValue;

        public string GetString(string key) => Convert.ToString(_data[key]) ?? string.Empty;
        public long GetLong(string key) => Convert.ToInt64(_data[key]);
        public double GetDouble(string key) => Convert.ToDouble(_data[key]);
        public bool GetBool(string key) => Convert.ToBoolean(_data[key]);

        public bool Unset(string key) => _data.Remove(key);
        public void ClearData() => _data.Clear();
    }
}