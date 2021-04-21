using Newtonsoft.Json;
using RPGDataEditor.Services;
using System;

namespace RPGDataEditor.Core.Services
{
    public class NewtonsoftSerializer : ISerializer
    {
        public object Deserialize(string value, Type type = null) => JsonConvert.DeserializeObject(value, type);

        public string Serialize(object obj, Type type = null) => JsonConvert.SerializeObject(obj, type, null);
    }
}
