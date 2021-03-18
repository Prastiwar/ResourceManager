using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics.CodeAnalysis;

namespace RPGDataEditor.Core.Serialization
{
    public class AbstractClassJsonConverter<T> : JsonConverter<T>
    {
        public AbstractClassJsonConverter(params string[] namespaceNames) => NamespaceNames = namespaceNames ?? throw new ArgumentNullException(nameof(namespaceNames));

        protected string[] NamespaceNames { get; set; }

        protected virtual string Suffix { get; }

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

        protected virtual string GetTypeName(T src) => string.IsNullOrEmpty(Suffix) ? src.GetType().Name 
                                                                                    : src.GetType().Name.Replace(Suffix, "");

        protected virtual Type GetObjectType(string type)
        {
            string suffixType = string.IsNullOrEmpty(Suffix) ? "." + type 
                                                             : "." + type + Suffix;
            foreach (string namespaceName in NamespaceNames)
            {
                Type foundType = Type.GetType(namespaceName + suffixType);
                if (foundType != null)
                {
                    return foundType;
                }
            }
            return null;
        }
    }
}
