using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ToolQit.Extensions;

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

        public Stream SerializeJson()
        {
            Stream jsonBuffer = new MemoryStream();
            if (Containers.Count == 0 && _data.Count == 0) return jsonBuffer;
            
            using Utf8JsonWriter jsonWriter = new Utf8JsonWriter(jsonBuffer);
            jsonWriter.WriteStartObject();
            foreach (var kvpData in _data)
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
                
            foreach (var kvpCollection in Containers)
            {
                var serColl = kvpCollection.Value.SerializeJson() as MemoryStream;
                if (serColl == null || serColl.Length == 0) continue;
                jsonWriter.WritePropertyName(kvpCollection.Key);
                jsonWriter.WriteRawValue(serColl.ToArray());
            }
            jsonWriter.WriteEndObject();
            return jsonBuffer;
        }

        public void DeserializeJson(Stream jsonStream)
        {
            ReadOnlySpan<byte> jsonBytes = new ReadOnlySpan<byte>((jsonStream as MemoryStream).ToArray());
            Utf8JsonReader jsonReader = new Utf8JsonReader(jsonBytes);
            jsonReader.Read();
            if (jsonReader.TokenType != JsonTokenType.StartObject) return;
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
                            Set(propName, stringValue);
                            break;
                        case JsonTokenType.Number:
                            if (jsonReader.TryGetDouble(out double dValue))
                            {
                                Set(propName, dValue);
                                continue;
                            }
                            if (jsonReader.TryGetInt64(out long lValue))
                                Set(propName, lValue);
                            break;
                        case JsonTokenType.True:
                        case JsonTokenType.False:
                            Set(propName, jsonReader.GetBoolean());
                            break;
                        case JsonTokenType.StartObject:
                            using (JsonDocument jsonDoc = JsonDocument.ParseValue(ref jsonReader))
                            {
                                DataContainer? subCollection = new DataContainer();
                                subCollection.DeserializeJson(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonDoc.RootElement.ToString())));
                                if (subCollection == null) continue;
                                AddContainer(propName, subCollection);
                            }
                            break;
                    }
                }
            }
        }
    }
}