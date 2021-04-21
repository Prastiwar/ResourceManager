using RPGDataEditor.Mvvm.Navigation;
using System.Threading.Tasks;

namespace RPGDataEditor.Mvvm.Services
{
    public interface IDialogService
    {
        Task<IDialogResult> ShowDialogAsync(object key, IDialogParameters parameters);
    }
}
