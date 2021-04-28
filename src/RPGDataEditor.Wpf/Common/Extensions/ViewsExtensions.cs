using RPGDataEditor.Extensions.Prism.Wpf;
using RPGDataEditor.Providers;
using RPGDataEditor.Wpf.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup.Primitives;
using System.Windows.Media;

namespace RPGDataEditor.Wpf
{
    public static class ViewsExtensions
    {
        public static void ChangeType<TModel>(this ChangeableUserControl.ChangeTypeEventArgs e, object sender)
        {
            if (sender is FrameworkElement element)
            {
                ListDataCard card = FindAncestorOrSelf<ListDataCard>(element);
                if (card?.ItemsSource is IList<TModel> list)
                {
                    ChangeTypeInList(e, list);
                }
                else
                {
                    object parentContext = null;
                    string contextPath = element.GetBindingExpression(FrameworkElement.DataContextProperty)?.ParentBinding.Path.Path;
                    DependencyObject parent = GetParent(element);
                    while (parentContext == null)
                    {
                        if (parent is FrameworkElement parentElement)
                        {
                            if (contextPath == null || contextPath == ".")
                            {
                                contextPath = parentElement.GetBindingExpression(FrameworkElement.DataContextProperty)?.ParentBinding.Path.Path;
                            }
                            if (parentElement.DataContext != element.DataContext)
                            {
                                parentContext = parentElement.DataContext;
                            }
                        }
                        parent = GetParent(parent);
                        if (parent == null)
                        {
                            break;
                        }
                    }
                    PropertyInfo property = parentContext.GetType().GetProperty(contextPath, BindingFlags.Public | BindingFlags.Instance);
                    if (property != null)
                    {
                        TModel newModel = CreateModel<TModel>(e);
                        property.SetValue(parentContext, newModel);
                    }
                }
            }
        }

        public static void ChangeTypeInList<TModel>(this ChangeableUserControl.ChangeTypeEventArgs e, IList<TModel> list, ItemsControl itemsControl = null)
        {
            TModel newModel = e.CreateModel<TModel>();
            if (newModel != null)
            {
                int index = list.IndexOf((TModel)e.Item);
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

        public static TModel CreateModel<TModel>(this ChangeableUserControl.ChangeTypeEventArgs e)
        {
            IImplementationProvider<TModel> implementationProvider = Application.Current.TryResolve<IImplementationProvider<TModel>>();
            if (implementationProvider != null)
            {
                return implementationProvider.Get(e.TargetType.Type);
            }
            return (TModel)Activator.CreateInstance(e.TargetType.Type);
        }

        public static void Refresh(this IEnumerable itemsSource) => CollectionViewSource.GetDefaultView(itemsSource)?.Refresh();

        public static void Refresh(this ItemsControl itemsControl) => itemsControl.ItemsSource?.Refresh();

        public static void AddValueChanged(this DependencyObject obj, DependencyProperty property, EventHandler handler)
        {
            DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(property, obj.GetType());
            dpd.AddValueChanged(obj, handler);
        }

        public static BindingExpressionBase SetBinding(this DependencyObject obj, DependencyProperty dp, BindingBase binding) => BindingOperations.SetBinding(obj, dp, binding);

        public static BindingExpression SetBinding(this DependencyObject obj, DependencyProperty dp, string path) => (BindingExpression)SetBinding(obj, dp, new Binding(path));

        public static T FindAncestorOrSelf<T>(DependencyObject obj) where T : DependencyObject
        {
            while (obj != null)
            {
                if (obj is T objTest)
                {
                    return objTest;
                }
                obj = GetParent(obj);
            }
            return null;
        }

        public static DependencyObject GetParent(DependencyObject obj)
        {
            if (obj == null)
            {
                return null;
            }
            if (obj is ContentElement ce)
            {
                DependencyObject parent = ContentOperations.GetParent(ce);
                if (parent != null)
                {
                    return parent;
                }
                return ce is FrameworkContentElement fce ? fce.Parent : null;
            }
            return VisualTreeHelper.GetParent(obj);
        }

        public static IEnumerable<DependencyProperty> EnumerateDependencyProperties(this DependencyObject element)
        {
            if (element != null)
            {
                MarkupObject markupObject = MarkupWriter.GetMarkupObjectFor(element);
                if (markupObject != null)
                {
                    foreach (MarkupProperty mp in markupObject.Properties)
                    {
                        if (mp.DependencyProperty != null)
                        {
                            yield return mp.DependencyProperty;
                        }
                    }
                }
            }
        }

        public static IEnumerable<DependencyProperty> EnumerateAttachedProperties(this DependencyObject element)
        {
            if (element != null)
            {
                MarkupObject markupObject = MarkupWriter.GetMarkupObjectFor(element);
                if (markupObject != null)
                {
                    foreach (MarkupProperty mp in markupObject.Properties)
                    {
                        if (mp.IsAttached)
                        {
                            yield return mp.DependencyProperty;
                        }
                    }
                }
            }
        }
    }
}
