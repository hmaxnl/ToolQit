using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ToolQit.Containers;
using ToolQit.Extensions;

namespace ToolQit.Serializers
{
    public class DataContainerJsonSerializer : ISerializer
    {
        #region Newtonsoft
        public bool Serialize(Stream stream, object data)
        {
            if (data is not DataContainer container) return false;
            if (container.Containers.Count == 0 && container.Data.Count == 0) return false;
            JsonWriter jsonWriter = new JsonTextWriter(new StreamWriter(stream))
            {
                Formatting = Formatting.Indented,
                CloseOutput = false,
                AutoCompleteOnClose = false
            };
            
            jsonWriter.WriteStartObject();
            foreach (var kvpData in container.Data)
            {
                jsonWriter.WritePropertyName(kvpData.Key);
                switch (kvpData.Value)
                {
                    case string sValue:
                        jsonWriter.WriteValue(sValue);
                        break;
                    case long lValue:
                        jsonWriter.WriteValue(lValue);
                        break;
                    case double dValue:
                        jsonWriter.WriteValue(dValue);
                        break;
                    case bool bValue:
                        jsonWriter.WriteValue(bValue);
                        break;
                }
            }
                
            foreach (var kvpCollection in container.Containers)
            {
                using MemoryStream serStream = new MemoryStream();
                if (!Serialize(serStream, kvpCollection.Value)) continue;
                if (serStream.Length == 0) continue;
                jsonWriter.WritePropertyName(kvpCollection.Key);
                // Tokenize the JSON so we keep the indentation.
                using JsonReader jsonTxtReader =
                    new JsonTextReader(new StringReader(Encoding.Default.GetString(serStream.ToArray())));
                jsonWriter.WriteToken(jsonTxtReader);
            }
            
            jsonWriter.WriteEndObject();
            jsonWriter.Flush();
            jsonWriter.Close();
            return true;
        }

        public bool Deserialize(Stream stream, out object output)
        {
            DataContainer container = new DataContainer();
            output = container;
            if (stream.Length == 0) return false;
            
            using MemoryStream jsonMemoryStream = new MemoryStream();
            stream.Position = 0;
            stream.CopyTo(jsonMemoryStream);

            jsonMemoryStream.Position = 0;
            StreamReader strReader = new StreamReader(jsonMemoryStream);
            using JsonReader jsonReader = new JsonTextReader(strReader);
            jsonReader.Read();
            if (jsonReader.TokenType != JsonToken.StartObject) return false;
            while (jsonReader.Read())
            {
                if (jsonReader.TokenType == JsonToken.PropertyName)
                {
                    string propName = jsonReader.Value?.ToString() ?? string.Empty;
                    if (propName.IsNullEmpty()) continue;
                    if (!jsonReader.Read()) return false;
                    switch (jsonReader.TokenType) // Value
                    {
                        case JsonToken.String:
                            string stringValue = jsonReader.Value?.ToString() ?? string.Empty;
                            if (stringValue.IsNullEmpty()) continue;
                            container.Set(propName, stringValue);
                            break;
                        case JsonToken.Float:
                            double dValue = Convert.ToInt64(jsonReader.Value); 
                            container.Set(propName, dValue);
                            break;
                        case JsonToken.Integer:
                            long lValue = Convert.ToInt32(jsonReader.Value);
                            container.Set(propName, lValue);
                            break;
                        case JsonToken.Boolean:
                            container.Set(propName, Convert.ToBoolean(jsonReader.Value));
                            break;
                        case JsonToken.StartObject:
                            JToken jTok = JToken.ReadFrom(jsonReader);
                            if (!Deserialize(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(jTok.ToString())), out object subColl) || subColl is not DataContainer subCollContainer)
                                continue;
                            container.AddContainer(propName, subCollContainer);
                            break;
                    }
                }
            }
            return true;
        }
        #endregion

        #region Sytem.Text.Json
        /*public bool Serialize(Stream stream, object data)
        {
            if (data is not DataContainer container) return false;
            if (container.Containers.Count == 0 && container.Data.Count == 0) return false;
            
            using Utf8JsonWriter jsonWriter = new Utf8JsonWriter(stream, new JsonWriterOptions() { Indented = true });
            jsonWriter.WriteStartObject();
            foreach (var kvpData in container.Data)
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
                
            foreach (var kvpCollection in container.Containers)
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

        public bool Deserialize(Stream stream, out object output)
        {
            DataContainer container = new DataContainer();
            output = container;
            if (stream.Length == 0) return false;
            
            using MemoryStream jsonMemoryStream = new MemoryStream();
            stream.Position = 0;
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
                            container.Set(propName, stringValue);
                            break;
                        case JsonTokenType.Number:
                            if (jsonReader.TryGetDouble(out double dValue))
                            {
                                container.Set(propName, dValue);
                                continue;
                            }
                            if (jsonReader.TryGetInt64(out long lValue))
                                container.Set(propName, lValue);
                            break;
                        case JsonTokenType.True:
                        case JsonTokenType.False:
                            container.Set(propName, jsonReader.GetBoolean());
                            break;
                        case JsonTokenType.StartObject:
                            using (JsonDocument jsonDoc = JsonDocument.ParseValue(ref jsonReader))
                            {
                                if (!Deserialize(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonDoc.RootElement.ToString())), out object subColl) || subColl is not DataContainer subCollContainer)
                                    continue;
                                container.AddContainer(propName, subCollContainer);
                            }
                            break;
                    }
                }
            }
            return true;
        }*/
        #endregion
    }
}