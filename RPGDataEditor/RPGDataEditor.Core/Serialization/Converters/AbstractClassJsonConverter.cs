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
            string typeName = obj.GetValue<string>("type");
            Type realType = GetObjectType(typeName);
            return (T)JsonConvert.DeserializeObject(obj.ToString(), realType);
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] T value, JsonSerializer serializer)
        {
            JObject obj = new JObject() {
                { "type", GetTypeName(value) }
            };
            obj.AddAfterSelf(value);
            obj.WriteTo(writer);
        }

        protected virtual string GetTypeName(T src) => src.GetType().Name;

        protected virtual Type GetObjectType(string type) => Type.GetType(namespaceName + "." + type);
    }
}
