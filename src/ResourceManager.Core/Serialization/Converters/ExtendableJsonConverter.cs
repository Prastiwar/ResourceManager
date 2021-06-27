
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ResourceManager.Core.Serialization
{
    public abstract class ExtendableJsonConverter<T> : JsonConverter<T>
    {
        public override T ReadJson(JsonReader reader, Type objectType, [AllowNull] T existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject obj = JObject.Load(reader);
                return ReadJObject(objectType, obj);
            }
            return default;
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] T value, JsonSerializer serializer)
        {
            JObject obj = ToJObject(value, serializer);
            obj.WriteTo(writer);
        }
        
        public abstract T ReadJObject(Type objectType, JObject obj);
        public abstract JObject ToJObject(T value, JsonSerializer serializer);
    }
}