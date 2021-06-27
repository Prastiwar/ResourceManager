using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace ResourceManager.Core.Serialization
{
    public class NullToEmptyStringResolver : DefaultPropertyResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty prop = base.CreateProperty(member, memberSerialization);
            if (prop.PropertyType == typeof(string))
            {
                prop.ValueProvider = new NullToEmptyStringValueProvider(prop.ValueProvider);
            }
            return prop;
        }
    }
}
