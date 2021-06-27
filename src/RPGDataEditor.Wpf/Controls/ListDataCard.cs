using ResourceManager;
using RPGDataEditor.Extensions.Prism.Wpf;
using RPGDataEditor.Providers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace RPGDataEditor.Wpf.Controls
{
    [ContentProperty(nameof(ItemContentTemplate))]
    public class ListDataCard : UserControl
    {
        public static DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IList), typeof(ListDataCard),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnItemsSourceChanged, OnItemsSourceCoerceValue));

        private static object OnItemsSourceCoerceValue(DependencyObject d, object baseValue)
        {
            object dataContext = d.GetValue(DataContextProperty);
            if (dataContext != null && baseValue == null)
            {
                BindingExpression expression = BindingOperations.GetBindingExpression(d, ItemsSourceProperty);
                if (expression?.ResolvedSource != null)
                {
                    PropertyDescriptor property = TypeDescriptor.GetProperties(expression.ResolvedSource).Find(expression.ResolvedSourcePropertyName, false);
                    if (property != null)
                    {
                        Type itemsType = property.PropertyType;
                        Type elementType = itemsType.GetEnumerableElementType();
                        object newInstance = null;
                        try
                        {
                            if (itemsType.IsAbstract || itemsType.IsInterface)
                            {
                                Type implementationType = typeof(List<>).MakeGenericType(elementType);
                                if (!itemsType.IsAssignableFrom(implementationType))
                                {
                                    if (itemsType.IsAssignableFrom(typeof(Collection<>).MakeGenericType(elementType)))
                                    {
                                        implementationType = typeof(Collection<>).MakeGenericType(elementType);
                                    }
                                    else if (itemsType.IsAssignableFrom(typeof(ObservableCollection<>).MakeGenericType(elementType)))
                                    {
                                        implementationType = typeof(ObservableCollection<>).MakeGenericType(elementType);
                                    }
                                    else
                                    {
                                        Type implementationProviderType = typeof(IImplementationProvider<>).MakeGenericType(itemsType);
                                        object provider = Application.Current.TryResolve(implementationProviderType);
                                        if (provider != null)
                                        {
                                            MethodInfo getMethod = implementationProviderType.GetMethod("Get", new Type[0]);
                                            implementationType = (Type)getMethod.Invoke(provider, null);
                                        }
                                    }
                                }
                                newInstance = Activator.CreateInstance(implementationType);
                            }
                            else
                            {
                                newInstance = Activator.CreateInstance(itemsType);
                            }
                            property.SetValue(expression.ResolvedSource, newInstance);
                            expression.UpdateTarget();
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex);
                            return baseValue;
                        }
                        return expression.Target.GetValue(expression.TargetProperty);
                    }
                }
            }
            return baseValue;
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ListDataCard).OnItemsSourceChanged();
        public IList ItemsSource {
            get => (IList)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static DependencyProperty AddItemCommandProperty =
            DependencyProperty.Register(nameof(AddItemCommand), typeof(ICommand), typeof(ListDataCard));
        public ICommand AddItemCommand {
            get => (ICommand)GetValue(AddItemCommandProperty);
            set => SetValue(AddItemCommandProperty, value);
        }

        public static DependencyProperty AddItemCommandParameterProperty =
            DependencyProperty.Register(nameof(AddItemCommandParameter), typeof(object), typeof(ListDataCard));
        public object AddItemCommandParameter {
            get => GetValue(AddItemCommandParameterProperty);
            set => SetValue(AddItemCommandParameterProperty, value);
        }

        public static DependencyProperty RemoveItemCommandProperty =
            DependencyProperty.Register(nameof(RemoveItemCommand), typeof(ICommand), typeof(ListDataCard));
        public ICommand RemoveItemCommand {
            get => (ICommand)GetValue(RemoveItemCommandProperty);
            set => SetValue(RemoveItemCommandProperty, value);
        }

        public static DependencyProperty ItemContentTemplateProperty =
            DependencyProperty.Register(nameof(ItemContentTemplate), typeof(DataTemplate), typeof(ListDataCard));
        public DataTemplate ItemContentTemplate {
            get => (DataTemplate)GetValue(ItemContentTemplateProperty);
            set => SetValue(ItemContentTemplateProperty, value);
        }

        public static DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(nameof(HeaderTemplate), typeof(DataTemplate), typeof(ListDataCard));
        public DataTemplate HeaderTemplate {
            get => (DataTemplate)GetValue(HeaderTemplateProperty);
            set => SetValue(HeaderTemplateProperty, value);
        }

        public static DependencyProperty HeaderTextProperty =
            DependencyProperty.Register(nameof(HeaderText), typeof(string), typeof(ListDataCard));
        public string HeaderText {
            get => (string)GetValue(HeaderTextProperty);
            set => SetValue(HeaderTextProperty, value);
        }

        public static DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(ListDataCard));
        public bool IsReadOnly {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public static DependencyProperty IsExpandableProperty =
            DependencyProperty.Register(nameof(IsExpandable), typeof(bool), typeof(ListDataCard), new PropertyMetadata(true));
        public bool IsExpandable {
            get => (bool)GetValue(IsExpandableProperty);
            set => SetValue(IsExpandableProperty, value);
        }

        public static DependencyProperty IsExpandedProperty =
            DependencyProperty.Register(nameof(IsExpanded), typeof(bool), typeof(ListDataCard), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool IsExpanded {
            get => (bool)GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }

        public static DependencyProperty NoExpandableVisibilityProperty =
            DependencyProperty.Register(nameof(NoExpandableVisibility), typeof(bool), typeof(ListDataCard));
        public bool NoExpandableVisibility {
            get => (bool)GetValue(NoExpandableVisibilityProperty);
            set => SetValue(NoExpandableVisibilityProperty, value);
        }

        private Type elementType;

        protected virtual void OnItemsSourceChanged() => elementType = ItemsSource?.GetType().GetArrayElementType();

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            OnTemplateApplied();
        }

        protected virtual void OnTemplateApplied()
        {
            if (HeaderTemplate == null && GetBindingExpression(HeaderTemplateProperty) == null)
            {
                HeaderTemplate = TemplateGenerator.CreateDataTemplate(() => {
                    TextBlock textBlock = new TextBlock();
                    textBlock.SetBinding(TextBlock.TextProperty, new Binding(nameof(HeaderText)) { Source = this });
                    return textBlock;
                });
            }
            if (RemoveItemCommand == null && !this.HasBinding(RemoveItemCommandProperty))
            {
                RemoveItemCommand = Commands.RemoveListItemCommand(() => ItemsSource);
            }
            if (AddItemCommand == null && !this.HasBinding(AddItemCommandProperty))
            {
                AddItemCommand = Commands.AddListItemCommand(() => ItemsSource, () => CreateDefaultElement());
            }
        }

        protected virtual object CreateDefaultElement()
        {
            try
            {
                return Activator.CreateInstance(elementType);
            }
            catch (Exception)
            {
                Type implementationProviderType = typeof(IImplementationProvider<>).MakeGenericType(elementType);
                object provider = Application.Current.TryResolve(implementationProviderType);
                if (provider == null)
                {
                    return null;
                }
                MethodInfo getMethod = implementationProviderType.GetMethod("Get", new Type[0]);
                return getMethod.Invoke(provider, null);
            }
        }
    }
}
