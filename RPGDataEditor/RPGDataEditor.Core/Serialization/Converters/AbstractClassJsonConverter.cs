using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics.CodeAnalysis;

namespace RPGDataEditor.Core.Serialization
{
    public class AbstractClassJsonConverter<T> : JsonConverter<T>
    {
        protected string namespaceName;

        public AbstractClassJsonConverter(string namespaceName) => this.namespaceName = namespaceName;

        public override T ReadJson(JsonReader reader, Type objectType, [AllowNull] T existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject obj = reader.ToJObject();
            if (obj == null)
            {
                return default;
            }
            string typeName = obj.GetValue<string>("type");
            Type realType = GetObjectType(typeName);
            serializer.Converters.Remove(this);
            T item = (T)obj.ToObject(realType, serializer);
            serializer.Converters.Add(this);
            return item;
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] T value, JsonSerializer serializer)
        {
            serializer.Converters.Remove(this);
            JObject obj = JObject.FromObject(value, serializer);
            serializer.Converters.Add(this);
            obj.AddFirst(new JProperty("type", GetTypeName(value)));
            obj.WriteTo(writer);
        }

        protected virtual string GetTypeName(T src) => src.GetType().Name;

        protected virtual Type GetObjectType(string type) => Type.GetType(namespaceName + "." + type);
    }
}
