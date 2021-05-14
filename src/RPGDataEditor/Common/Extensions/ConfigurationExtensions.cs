using Microsoft.Extensions.Configuration;
using RPGDataEditor.Services;
using System.Threading.Tasks;

namespace RPGDataEditor
{
    public static class ConfigurationExtensions
    {
        public static Task SaveConfigAsync(this IAppPersistanceService service, IConfigurationSection section) => service.SaveAsync("session", section);
    }
}
