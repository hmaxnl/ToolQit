using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using ToolQit.Collections;

namespace ToolQit.Converters
{
    public class DataCollectionJsonConverter : JsonConverter<DataCollection>
    {
        public override DataCollection? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException(); //TODO: Implement Read!
        }

        public override void Write(Utf8JsonWriter writer, DataCollection value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            foreach (var kvpData in value._data)
            {
                switch (kvpData.Value)
                {
                    case string sValue:
                        writer.WriteString(kvpData.Key, sValue);
                        break;
                    case int iValue:
                        writer.WriteNumber(kvpData.Key, iValue);
                        break;
                    case double dValue:
                        writer.WriteNumber(kvpData.Key, dValue);
                        break;
                    case bool bValue:
                        writer.WriteBoolean(kvpData.Key, bValue);
                        break;
                }
            }

            foreach (var kvpCollection in value._collections)
            {
                writer.WritePropertyName(kvpCollection.Key);
                var serColl = kvpCollection.Value.ToJson();
                writer.WriteRawValue(serColl);
            }
            writer.WriteEndObject();
        }
    }
}