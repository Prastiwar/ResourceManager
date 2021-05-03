using ResourceManager.Services;
using System.Threading.Tasks;

namespace RPGDataEditor.Services
{
    public interface IAppPersistanceService
    {
        Task SaveAsync(string name, object obj);
    }
}
