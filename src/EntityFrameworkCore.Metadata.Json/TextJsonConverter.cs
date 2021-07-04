using System.Text.Json;

namespace EntityFrameworkCore.Metadata.Json
{
    public class TextJsonConverter<T> : IJsonValueConverter<T>
    {
        public TextJsonConverter(JsonSerializerOptions options) => Options = options;

        protected JsonSerializerOptions Options { get; }

        public string Serialize(T value) => JsonSerializer.Serialize<T>(value, Options);

        public T Deserialize(string value) => JsonSerializer.Deserialize<T>(value, Options);
    }
}
