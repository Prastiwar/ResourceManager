using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Core.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace RPGDataEditor.Core.Serialization
{
    public class AttributeDataModelJsonConverter : JsonConverter<AttributeDataModel>
    {
        public override AttributeDataModel ReadJson(JsonReader reader, Type objectType, [AllowNull] AttributeDataModel existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject obj = JObject.Load(reader);
                string name = obj.GetValue<string>(nameof(AttributeDataModel.Name));
                double value = obj.GetValue<double>(nameof(AttributeDataModel.Value), 0.0);
                AttributeDataModel model = new AttributeDataModel() {
                    Name = name,
                    Value = value
                };
                return model;
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] AttributeDataModel value, JsonSerializer serializer)
        {
            JObject obj = new JObject() {
                { nameof(AttributeDataModel.Name).ToFirstLower(), value.Name },
                { nameof(AttributeDataModel.Value).ToFirstLower(), value.Value }
            };
            obj.WriteTo(writer);
        }
    }
}
