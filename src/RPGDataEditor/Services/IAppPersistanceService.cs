using System.Threading.Tasks;

namespace RPGDataEditor.Services
{
    public interface IAppPersistanceService
    {
        Task SaveAsync(string name, object obj);
        void Save(string name, object obj);
    }
}
