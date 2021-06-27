using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Extensions.Configuration.WriteableJson
{
    public class WritableJsonConfigurationSource : JsonConfigurationSource
    {
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            EnsureDefaults(builder);
            return new WritableJsonConfigurationProvider(this);
        }
    }
}
