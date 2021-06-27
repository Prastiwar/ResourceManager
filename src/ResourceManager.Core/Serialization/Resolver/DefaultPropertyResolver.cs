using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace ResourceManager.Core.Serialization
{
    public class DefaultPropertyResolver : DefaultContractResolver
    {
        public virtual JsonProperty CreateJsonProperty(MemberInfo member, MemberSerialization memberSerialization)
            => CreateProperty(member, memberSerialization);
    }
}
