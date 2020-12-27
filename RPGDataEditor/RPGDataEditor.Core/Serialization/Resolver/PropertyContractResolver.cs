using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RPGDataEditor.Core.Serialization
{
    public enum Lettercase { Default, Lowercase, Uppercase }

    public class PropertyContractResolver : DefaultPropertyResolver
    {
        private readonly Dictionary<Type, HashSet<string>> ignored = new Dictionary<Type, HashSet<string>>();
        private readonly Dictionary<Type, Dictionary<string, string>> renamed = new Dictionary<Type, Dictionary<string, string>>();
        private Lettercase lettercase;
        private bool stringifyNull;

        public void IgnoreProperty(Type fromClass, params string[] jsonPropertyNames)
        {
            if (!ignored.ContainsKey(fromClass))
            {
                ignored[fromClass] = new HashSet<string>();
            }

            foreach (string prop in jsonPropertyNames)
            {
                ignored[fromClass].Add(prop);
            }
        }

        public void RenameProperty(Type fromClass, string propertyName, string newJsonPropertyName)
        {
            if (!renamed.ContainsKey(fromClass))
            {
                renamed[fromClass] = new Dictionary<string, string>();
            }
            renamed[fromClass][propertyName] = newJsonPropertyName;
        }

        public void SetAllLetterCase(Lettercase lettercase) => this.lettercase = lettercase;

        public void SetNullStringEmpty(bool shouldMakeNullStringEmpty) => stringifyNull = shouldMakeNullStringEmpty;

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (IsIgnored(property.DeclaringType, property.PropertyName))
            {
                property.ShouldSerialize = obj => false;
                property.Ignored = true;
            }
            else if (IsRenamed(property.DeclaringType, property.PropertyName, out string newJsonPropertyName))
            {
                property.PropertyName = newJsonPropertyName;
            }
            switch (lettercase)
            {
                case Lettercase.Lowercase:
                    property.PropertyName = property.PropertyName.ToLower();
                    break;
                case Lettercase.Uppercase:
                    property.PropertyName = property.PropertyName.ToUpper();
                    break;
                default:
                    break;
            }
            if (stringifyNull && property.PropertyType == typeof(string))
            {
                property.ValueProvider = new NullToEmptyStringValueProvider(property.ValueProvider);
            }
            return property;
        }

        protected bool IsIgnored(Type type, string jsonPropertyName)
        {
            Type ignoredType = type;
            while (ignoredType != null && !ignored.ContainsKey(ignoredType))
            {
                ignoredType = ignoredType.BaseType;
            }
            return ignoredType != null && ignored[ignoredType].Contains(jsonPropertyName);
        }

        protected bool IsRenamed(Type fromClass, string jsonPropertyName, out string newJsonPropertyName)
        {
            newJsonPropertyName = null;
            Type renamedType = fromClass;
            while (renamedType != null && !renamed.ContainsKey(renamedType))
            {
                renamedType = renamedType.BaseType;
            }
            return renamedType != null && renamed[renamedType].TryGetValue(jsonPropertyName, out newJsonPropertyName);
        }
    }
}
