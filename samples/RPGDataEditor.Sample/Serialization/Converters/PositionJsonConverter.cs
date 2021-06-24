using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Core;
using RPGDataEditor.Sample.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace RPGDataEditor.Sample.Serialization
{
    public class PositionJsonConverter : JsonConverter<Position>
    {
        public override Position ReadJson(JsonReader reader, Type objectType, [AllowNull] Position existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject obj = JObject.Load(reader);
                int x = obj.GetValue<int>(nameof(Position.X), 0);
                int y = obj.GetValue<int>(nameof(Position.Y), 0);
                int z = obj.GetValue<int>(nameof(Position.Z), 0);
                Position model = new Position() {
                    X = x,
                    Y = y,
                    Z = z
                };
                return model;
            }
            return default;
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] Position value, JsonSerializer serializer)
        {
            JObject obj = new JObject() {
                { nameof(Position.X).ToFirstLower(), value.X },
                { nameof(Position.Y).ToFirstLower(), value.Y },
                { nameof(Position.Z).ToFirstLower(), value.Z }
            };
            obj.WriteTo(writer);
        }
    }
}
