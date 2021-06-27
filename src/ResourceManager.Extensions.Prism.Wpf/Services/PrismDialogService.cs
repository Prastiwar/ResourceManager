using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;

namespace ResourceManager.Extensions.Prism.Wpf.Services
{
    public class PrismDialogService : Mvvm.Services.IDialogService
    {
        private readonly IDialogService prismService;

        public PrismDialogService(IDialogService prismService) => this.prismService = prismService;

        public Task<Mvvm.Navigation.IDialogResult> ShowDialogAsync(object key, Mvvm.Navigation.IDialogParameters parameters)
        {
            TaskCompletionSource<Mvvm.Navigation.IDialogResult> tcs = new TaskCompletionSource<Mvvm.Navigation.IDialogResult>();
            try
            {
                prismService.ShowDialog(key.ToString(), parameters?.ToPrism(), (result) => tcs.TrySetResult(result.ToDomain()));
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
            return tcs.Task;
        }
    }
}
