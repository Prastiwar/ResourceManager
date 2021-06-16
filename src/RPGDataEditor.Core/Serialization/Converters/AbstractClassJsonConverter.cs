using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace RPGDataEditor.Core.Serialization
{
    public class AbstractClassJsonConverter<T> : JsonConverter<T>
    {
        public AbstractClassJsonConverter(Assembly[] assemblies, params string[] namespaceNames)
        {
            Assemblies = assemblies;
            NamespaceNames = namespaceNames ?? throw new ArgumentNullException(nameof(namespaceNames));
        }

        protected Assembly[] Assemblies { get; set; }
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
            if (realType != null)
            {
                serializer.Converters.Remove(this);
                T item = (T)obj.ToObject(realType, serializer);
                serializer.Converters.Add(this);
                return item;
            }
            return default;
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
            if (Assemblies == null)
            {
                return GetObjectType(suffixType, typeof(AbstractClassJsonConverter<T>).Assembly.FullName);
            }
            foreach (Assembly assembly in Assemblies)
            {
                Type foundType = GetObjectType(suffixType, assembly.FullName);
                if (foundType != null)
                {
                    return foundType;
                }
            }
            return null;
        }

        protected Type GetObjectType(string type, string assemblyName)
        {
            string typeWithAssemblyName = type + ", " + assemblyName;
            foreach (string namespaceName in NamespaceNames)
            {
                Type foundType = Type.GetType(namespaceName + typeWithAssemblyName);
                if (foundType != null)
                {
                    return foundType;
                }
            }
            return null;
        }
    }
}
