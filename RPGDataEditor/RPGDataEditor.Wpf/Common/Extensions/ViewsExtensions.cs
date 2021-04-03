using Prism;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Providers;
using RPGDataEditor.Wpf.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RPGDataEditor.Wpf
{
    public static class ViewsExtensions
    {
        public static void AddValueChanged(this DependencyObject obj, DependencyProperty property, EventHandler handler)
        {
            DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(property, obj.GetType());
            dpd.AddValueChanged(obj, handler);
        }

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
                if (itemsControl != null)
                {
                    itemsControl.Refresh();
                }
                else
                {
                    list.Refresh();
                }
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

        public static void Refresh(this IEnumerable itemsSource) => CollectionViewSource.GetDefaultView(itemsSource)?.Refresh();

        public static void Refresh(this ItemsControl itemsControl) => itemsControl.ItemsSource?.Refresh();
    }
}
