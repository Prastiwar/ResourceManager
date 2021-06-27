using Extensions.Configuration.WriteableJson;

namespace Microsoft.Extensions.Configuration
{
    public static class WriteableJsonConfigurationExtensions
    {
        public static IConfigurationBuilder AddWriteableJsonFile(this IConfigurationBuilder builder, string path, bool optional = false, bool reloadOnChange = true)
            => builder.Add<WritableJsonConfigurationSource>(s => {
                s.FileProvider = null;
                s.Path = path;
                s.Optional = optional;
                s.ReloadOnChange = reloadOnChange;
                s.ResolveFileProvider();
            });
    }
}
