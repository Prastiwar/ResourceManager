using Newtonsoft.Json;

namespace EntityFrameworkCore.Metadata.Json
{
    public class NewtonsoftJsonConverter<T> : IJsonValueConverter<T>
    {
        public NewtonsoftJsonConverter(JsonSerializerSettings settings = null) => Settings = settings;

        public JsonSerializerSettings Settings { get; }

        public string Serialize(T value) => JsonConvert.SerializeObject(value, Formatting.None, Settings);

        public T Deserialize(string json) => JsonConvert.DeserializeObject<T>(json);
    }
}
