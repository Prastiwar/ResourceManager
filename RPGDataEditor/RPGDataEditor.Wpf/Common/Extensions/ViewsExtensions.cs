using Prism;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Providers;
using RPGDataEditor.Wpf.Controls;
using System.Windows;

namespace RPGDataEditor.Wpf
{
    public static class ViewsExtensions
    {
        public static TModel CreateModel<TModel>(this ChangeableUserControl.ChangeTypeEventArgs e) where TModel : ObservableModel
        {
            if (Application.Current is PrismApplicationBase prismApp)
            {
                if (prismApp.Container.Resolve(typeof(IModelProvider<TModel>)) is IModelProvider<TModel> provider)
                {
                    return provider.CreateModel(e.TargetType);
                }
            }
            return null;
        }
    }
}
