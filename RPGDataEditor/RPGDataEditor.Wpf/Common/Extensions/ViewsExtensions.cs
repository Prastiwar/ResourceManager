using Prism;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Providers;
using RPGDataEditor.Wpf.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RPGDataEditor.Wpf
{
    public static class ViewsExtensions
    {
        public static object TryResolve(this Application app, Type type) => app is PrismApplicationBase prismApp ? prismApp.Container.Resolve(type) : default;

        public static T TryResolve<T>(this Application app) => TryResolve(app, typeof(T)) is T item ? item : default;

        public static void ChangeTypeInList<TModel>(this ChangeableUserControl.ChangeTypeEventArgs e, IList<TModel> list, ItemsControl itemsControl = null)
            where TModel : ObservableModel
        {
            TModel newModel = e.CreateModel<TModel>();
            if (newModel != null)
            {
                int index = list.IndexOf(e.Item as TModel);
                if (index > -1)
                {
                    list.RemoveAt(index);
                    list.Insert(index, newModel);
                }
                itemsControl?.Refresh();
            }
        }

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

        public static void Refresh(this ItemsControl itemsControl) => CollectionViewSource.GetDefaultView(itemsControl.ItemsSource)?.Refresh();
    }
}
