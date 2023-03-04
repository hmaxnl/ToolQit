using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using ToolQit.Collections;
using ToolQit.Extensions;

namespace ToolQit.Converters
{
    public class DataCollectionJsonConverter : JsonConverter<DataCollection>
    {
        public override DataCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException();
            DataCollection collectionJson = new DataCollection();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propName = reader.GetString() ?? string.Empty;
                    if (propName.IsNullEmpty()) continue;
                    reader.Read();
                    switch (reader.TokenType) // Value
                    {
                        case JsonTokenType.String:
                            string stringValue = reader.GetString() ?? string.Empty;
                            if (stringValue.IsNullEmpty()) continue;
                            collectionJson.Set(propName, stringValue);
                            break;
                        case JsonTokenType.Number:
                            if (reader.TryGetDouble(out double dValue))
                            {
                                collectionJson.Set(propName, dValue);
                                continue;
                            }
                            if (reader.TryGetInt64(out long lValue))
                                collectionJson.Set(propName, lValue);
                            break;
                        case JsonTokenType.True:
                        case JsonTokenType.False:
                            collectionJson.Set(propName, reader.GetBoolean());
                            break;
                        case JsonTokenType.StartObject:
                            using (JsonDocument jsonDoc = JsonDocument.ParseValue(ref reader))
                            {
                                DataCollection? subCollection = JsonSerializer.Deserialize<DataCollection>(jsonDoc.RootElement.GetRawText(), collectionJson.CollectionJsonOptions);
                                if (subCollection == null) continue;
                                collectionJson.AddCollection(propName, subCollection);
                            }
                            break;
                    }
                }
            }
            return collectionJson;
        }

        public override void Write(Utf8JsonWriter writer, DataCollection value, JsonSerializerOptions options)
        {
            var dataCollections = value.DataCollections;
            var data = value.Data;
            
            if (dataCollections.Count == 0 && data.Count == 0) return; // Skip the collection if it is empty, so we dont write it to JSON.
            // JSON
            writer.WriteStartObject();
            foreach (var kvpData in data)
            {
                switch (kvpData.Value)
                {
                    case string sValue:
                        writer.WriteString(kvpData.Key, sValue);
                        break;
                    case long lValue:
                        writer.WriteNumber(kvpData.Key, lValue);
                        break;
                    case double dValue:
                        writer.WriteNumber(kvpData.Key, dValue);
                        break;
                    case bool bValue:
                        writer.WriteBoolean(kvpData.Key, bValue);
                        break;
                }
            }

            foreach (var kvpCollection in dataCollections)
            {
                writer.WritePropertyName(kvpCollection.Key);
                var serColl = kvpCollection.Value.ToJson();
                writer.WriteRawValue(serColl);
            }
            writer.WriteEndObject();
        }
    }
}