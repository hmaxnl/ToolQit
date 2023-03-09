using System;
using System.IO;
using System.Text;
using ToolQit.Serializers;

namespace ToolQit.Tools
{
    public static class Serializer
    {
        public static string Serialize(object data, ISerializer serializer)
        {
            using MemoryStream serStream = new MemoryStream();
            if (!serializer.Serialize(serStream, data) || serStream.Length == 0) return string.Empty;
            return Convert.ToString(Encoding.UTF8.GetString(serStream.ToArray()));
        }

        public static TObject? Deserialize<TObject>(string data, ISerializer serializer)
        {
            MemoryStream dataStream = new MemoryStream(Encoding.UTF8.GetBytes(data));
            if (serializer.Deserialize(dataStream, out object output) && output is TObject obj) return obj;
            return default;
        }
    }
}