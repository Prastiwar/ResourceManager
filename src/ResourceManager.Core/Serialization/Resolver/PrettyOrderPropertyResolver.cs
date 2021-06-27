using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResourceManager.Core.Serialization
{
    /// <summary> Changes order to: 'id' first, iterables last  </summary>
    public class PrettyOrderPropertyResolver : PropertyContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);
            if (properties == null)
            {
                return null;
            }
            JsonProperty idProperty = properties.FirstOrDefault(p => string.Compare(p.PropertyName, "id", true) == 0);
            if (idProperty != null)
            {
                properties.Remove(idProperty);
                properties.Insert(0, idProperty);
            }
            return properties.OrderBy(p => typeof(IEnumerable<object>).IsAssignableFrom(p.PropertyType)).ToList();
        }
    }
}
