using RPGDataEditor.Mvvm.Models;
using RPGDataEditor.Mvvm.Navigation;
using RPGDataEditor.Mvvm.Services;
using System.Threading.Tasks;

namespace RPGDataEditor.Mvvm
{
    public static class DialogServiceExtensions
    {
        public static T GetValue<T>(this IDialogParameters parameters, string parameter) => (T)parameters.GetValue(parameter);

        public static Task<bool> ShowModelDialogAsync<TModel>(this IDialogService dialogService, TModel model) where TModel : ObservableModel
            => dialogService.ShowModelDialogAsync(typeof(TModel).Name, model);

        public static async Task<bool> ShowModelDialogAsync<TModel>(this IDialogService dialogService, string name, TModel model) where TModel : ObservableModel
        {
            IDialogResult result = await dialogService.ShowModelDialogWithResultAsync(name, model);
            return result.Parameters.GetValue<bool>(nameof(ModelDialogParameters<TModel>.IsSuccess));
        }
        public static Task<IDialogResult> ShowModelDialogWithResultAsync<TModel>(this IDialogService dialogService, TModel model) where TModel : ObservableModel
            => dialogService.ShowDialogAsync(typeof(TModel).Name, new ModelDialogParameters<TModel>(model).Build());

        public static Task<IDialogResult> ShowModelDialogWithResultAsync<TModel>(this IDialogService dialogService, string name, TModel model) where TModel : ObservableModel
            => dialogService.ShowDialogAsync(name, new ModelDialogParameters<TModel>(model).Build());
    }
}
