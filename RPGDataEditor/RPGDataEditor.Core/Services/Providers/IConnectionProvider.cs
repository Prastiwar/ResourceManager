using RPGDataEditor.Core.Services;

namespace RPGDataEditor.Core.Providers
{
    public interface IConnectionProvider
    {
        IConnectionService GetConnectionService(string name);
    }

}