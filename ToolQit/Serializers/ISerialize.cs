using System.IO;

namespace ToolQit.Serializers
{
    public interface ISerialize<TObject>
    {
        public bool Serialize(Stream stream, TObject data);

        public bool Deserialize(Stream stream, out TObject output);
    }
}