using RPGDataEditor.Mvvm.Navigation;
using RPGDataEditor.Mvvm.Services;
using System.Threading.Tasks;

namespace RPGDataEditor.Mvvm
{
    public static class DialogExtensions
    {
        public static T GetValue<T>(this IDialogParameters parameters, string parameter) => (T)parameters.GetValue(parameter);

        public static Task<IDialogResult> ShowDialogAsync(this IDialogService dialogService, string name) => dialogService.ShowDialogAsync(name, null);

        public static Task<bool> ShowModelDialogAsync<TModel>(this IDialogService dialogService, TModel model)
            => dialogService.ShowModelDialogAsync(typeof(TModel).Name, model);

        public static async Task<bool> ShowModelDialogAsync<TModel>(this IDialogService dialogService, string name, TModel model)
        {
            IDialogResult result = await dialogService.ShowModelDialogWithResultAsync(name, model);
            try
            {
                return result.Parameters.GetValue<bool>(nameof(ModelDialogParameters<TModel>.IsSuccess));
            }
            catch (System.Exception)
            {
                return false;
            }
        }
        public static Task<IDialogResult> ShowModelDialogWithResultAsync<TModel>(this IDialogService dialogService, TModel model)
            => dialogService.ShowDialogAsync(typeof(TModel).Name, new ModelDialogParameters<TModel>(model).Build());

        public static Task<IDialogResult> ShowModelDialogWithResultAsync<TModel>(this IDialogService dialogService, string name, TModel model)
            => dialogService.ShowDialogAsync(name, new ModelDialogParameters<TModel>(model).Build());
    }
}
