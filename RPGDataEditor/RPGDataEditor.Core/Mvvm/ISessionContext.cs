using RPGDataEditor.Core.Connection;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Providers;
using RPGDataEditor.Core.Validation;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Mvvm
{
    public interface ISessionContext : IValidable
    {
        IResourceClient Client { get; set; }
        IClientProvider ClientProvider { get; set; }

        void SetConnection(string name);

        void OnResourceChanged(RPGResource resource);

        Task<bool> CreateBackupAsync(RPGResource resource);

        void SaveSession();

        ISessionContext LoadSession();
    }
}
