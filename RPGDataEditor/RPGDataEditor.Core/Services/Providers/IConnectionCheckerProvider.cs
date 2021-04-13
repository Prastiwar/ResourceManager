using RPGDataEditor.Core.Connection;

namespace RPGDataEditor.Core.Providers
{
    public interface IConnectionCheckerProvider
    {
        IConnectionChecker GetConnectionChecker(IResourceClient client);
    }
}
