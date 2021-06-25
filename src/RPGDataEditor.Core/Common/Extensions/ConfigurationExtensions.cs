using Microsoft.Extensions.Configuration;
using RPGDataEditor.Services;
using System.Threading.Tasks;

namespace RPGDataEditor
{
    public static class ConfigurationExtensions
    {
        public const string SessionFileName = "session";

        public static Task SaveConfigAsync(this IAppPersistanceService service, IConfiguration configuration) => service.SaveAsync(SessionFileName, configuration);

        public static void SaveConfig(this IAppPersistanceService service, IConfiguration configuration) => service.Save(SessionFileName, configuration);
    }
}
