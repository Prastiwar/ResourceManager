using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace ResourceManager.Core
{
    public static class NewtonsoftJsonExtensions
    {
        public static JObject ToJObject(this JsonReader reader)
        {
            try
            {
                JObject obj = reader.Value is string stringValue ? JObject.Parse(stringValue) : JObject.Load(reader);
                return obj;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static T GetValue<T>(this JObject obj, string name, T defaultValue, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
            => obj.TryGetValue(name, comparison, out JToken value) ? value.ToObject<T>() : defaultValue;

        public static T GetValue<T>(this JObject obj, string name, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
            where T : class => obj.GetValue(name, comparison)?.ToObject<T>();

        public static T GetValue<T>(this JObject obj, StringComparison comparison, params string[] allowedNames)
        {
            if (allowedNames != null && allowedNames.Length > 0)
            {
                for (int i = 0; i < allowedNames.Length; i++)
                {
                    if (obj.TryGetValue(allowedNames[i], comparison, out JToken token))
                    {
                        return token.ToObject<T>();
                    }
                }
            }
            return default;
        }
    }
}
