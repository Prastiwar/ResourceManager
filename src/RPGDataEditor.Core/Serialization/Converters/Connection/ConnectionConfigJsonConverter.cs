using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Connection;
using System;

namespace RPGDataEditor.Core.Serialization
{
    public class ConnectionConfigJsonConverter : ExtendableJsonConverter<IConnectionConfig>
    {
        public override IConnectionConfig ReadJObject(Type objectType, JObject obj)
        {
            ConnectionSettings settings = new ConnectionSettings();
            foreach (System.Collections.Generic.KeyValuePair<string, JToken> item in obj)
            {
                settings.Set(item.Key, item.Value.ToObject<object>());
            }
            return settings.CreateConfig();
        }

        public override JObject ToJObject(IConnectionConfig value, JsonSerializer serializer)
        {
            JObject obj = new JObject();
            foreach (System.Collections.Generic.KeyValuePair<string, object> item in value)
            {
                obj.Add(item.Key, JToken.FromObject(item.Value, serializer));
            }
            return obj;
        }
    }
}
