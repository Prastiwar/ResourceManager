using Prism.Services.Dialogs;
using RPGDataEditor.Core.Models;
using System;
using System.Threading.Tasks;

namespace RPGDataEditor.Core
{
    public static class DialogExtensions
    {
        public static Task<IDialogResult> ShowDialogAsync(this IDialogService dialogService, string name)
        {
            TaskCompletionSource<IDialogResult> tcs = new TaskCompletionSource<IDialogResult>();
            try
            {
                dialogService.ShowDialog(name, null, (result) => tcs.SetResult(result));
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
            return tcs.Task;
        }
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

        public static Task<IDialogResult> ShowDialogAsync(this IDialogService dialogService, string name, IDialogParameters parameters)
        {
            TaskCompletionSource<IDialogResult> tcs = new TaskCompletionSource<IDialogResult>();
            try
            {
                dialogService.ShowDialog(name, parameters, (result) => {
                    tcs.TrySetResult(result);
                });
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
            return tcs.Task;
        }

        public static Task<T> ShowDialogAsync<T>(this IDialogService dialogService, string name, IDialogParameters parameters, string resultName = null)
        {
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
            try
            {
                dialogService.ShowDialog(name, parameters, (result) => {
                    tcs.SetResult(result.Parameters.GetValue<T>(string.IsNullOrEmpty(resultName) ? typeof(T).Name : resultName));
                });
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
            return tcs.Task;
        }

        public static Task<T> ShowDialogAsync<T>(this IDialogService dialogService, string name)
        {
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
            try
            {
                dialogService.ShowDialog(name, null, (result) => {
                    tcs.SetResult(result.Parameters.GetValue<T>(typeof(T).Name));
                });
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
            return tcs.Task;
        }
    }
}
