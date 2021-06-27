using Newtonsoft.Json;
using ResourceManager.Data;
using System;

namespace ResourceManager.Core.Services
{
    public class NewtonsoftSerializer : ITextSerializer
    {
        public object Deserialize(string value, Type type = null) => JsonConvert.DeserializeObject(value, type);

        public string Serialize(object obj, Type type = null) => JsonConvert.SerializeObject(obj, type, null);
    }
}
