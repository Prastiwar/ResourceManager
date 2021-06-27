using ResourceManager.Mvvm.Navigation;
using System.Threading.Tasks;

namespace ResourceManager.Mvvm.Services
{
    public interface IDialogService
    {
        Task<IDialogResult> ShowDialogAsync(object key, IDialogParameters parameters);
    }
}
