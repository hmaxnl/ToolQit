using System.IO;

namespace ToolQit.Serializers
{
    public interface ISerializer
    {
        public bool Serialize(Stream stream, object data);

        public bool Deserialize(Stream stream, out object output);
    }
}