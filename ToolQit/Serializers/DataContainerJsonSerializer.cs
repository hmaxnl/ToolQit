using System.Buffers;
using System.IO;
using System.Text.Json;
using ToolQit.Containers;
using ToolQit.Extensions;

namespace ToolQit.Serializers
{
    public class DataContainerJsonSerializer : ISerialize<DataContainer>
    {
        public bool Serialize(Stream stream, DataContainer data)
        {
            if (data.Containers.Count == 0 && data.Data.Count == 0) return false;
            
            using Utf8JsonWriter jsonWriter = new Utf8JsonWriter(stream);
            jsonWriter.WriteStartObject();
            foreach (var kvpData in data.Data)
            {
                switch (kvpData.Value)
                {
                    case string sValue:
                        jsonWriter.WriteString(kvpData.Key, sValue);
                        break;
                    case long lValue:
                        jsonWriter.WriteNumber(kvpData.Key, lValue);
                        break;
                    case double dValue:
                        jsonWriter.WriteNumber(kvpData.Key, dValue);
                        break;
                    case bool bValue:
                        jsonWriter.WriteBoolean(kvpData.Key, bValue);
                        break;
                }
            }
                
            foreach (var kvpCollection in data.Containers)
            {
                MemoryStream serStream = new MemoryStream();
                if (!Serialize(serStream, kvpCollection.Value)) continue;
                if (serStream.Length == 0) continue;
                jsonWriter.WritePropertyName(kvpCollection.Key);
                jsonWriter.WriteRawValue(serStream.ToArray());
            }
            jsonWriter.WriteEndObject();
            return true;
        }

        public bool Deserialize(Stream stream, out DataContainer output)
        {
            output = new DataContainer();
            using MemoryStream jsonMemoryStream = new MemoryStream();
            stream.CopyTo(jsonMemoryStream);
            Utf8JsonReader jsonReader = new Utf8JsonReader(new ReadOnlySequence<byte>(jsonMemoryStream.ToArray()));
            jsonReader.Read();
            if (jsonReader.TokenType != JsonTokenType.StartObject) return false;
            while (jsonReader.Read())
            {
                if (jsonReader.TokenType == JsonTokenType.PropertyName)
                {
                    string propName = jsonReader.GetString() ?? string.Empty;
                    if (propName.IsNullEmpty()) continue;
                    jsonReader.Read();
                    switch (jsonReader.TokenType) // Value
                    {
                        case JsonTokenType.String:
                            string stringValue = jsonReader.GetString() ?? string.Empty;
                            if (stringValue.IsNullEmpty()) continue;
                            output.Set(propName, stringValue);
                            break;
                        case JsonTokenType.Number:
                            if (jsonReader.TryGetDouble(out double dValue))
                            {
                                output.Set(propName, dValue);
                                continue;
                            }
                            if (jsonReader.TryGetInt64(out long lValue))
                                output.Set(propName, lValue);
                            break;
                        case JsonTokenType.True:
                        case JsonTokenType.False:
                            output.Set(propName, jsonReader.GetBoolean());
                            break;
                        case JsonTokenType.StartObject:
                            using (JsonDocument jsonDoc = JsonDocument.ParseValue(ref jsonReader))
                            {
                                if (!Deserialize(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonDoc.RootElement.ToString())), out DataContainer subColl))
                                    continue;
                                output.AddContainer(propName, subColl);
                            }
                            break;
                    }
                }
            }
            return true;
        }
    }
}