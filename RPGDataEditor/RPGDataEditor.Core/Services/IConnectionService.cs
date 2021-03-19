using System.Threading.Tasks;

namespace RPGDataEditor.Core.Services
{
    public interface IConnectionService
    {
        Task<bool> ConnectAsync();
        Task<bool> DisconnectAsync();
    }
}
