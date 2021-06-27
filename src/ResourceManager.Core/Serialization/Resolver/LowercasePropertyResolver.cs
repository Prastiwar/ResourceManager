using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace ResourceManager.Core.Serialization
{
    public class LowercasePropertyResolver : DefaultPropertyResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            property.PropertyName = property.PropertyName.ToLower();
            return property;
        }
    }
}
