using RPGDataEditor.Core.Connection;

namespace RPGDataEditor.Core.Providers
{
    public interface IClientProvider
    {
        IResourceClient GetClient(string name);
    }

}