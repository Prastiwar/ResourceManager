using RPGDataEditor.Connection;
using RPGDataEditor.Services;
using System.Threading.Tasks;

namespace RPGDataEditor.Wpf
{
    public static class ConnectionExtensions
    {
        public static Task SaveConfigAsync(this IAppPersistanceService service, IConnectionConfig config) => service.SaveAsync("Connection", config);
    }
}
