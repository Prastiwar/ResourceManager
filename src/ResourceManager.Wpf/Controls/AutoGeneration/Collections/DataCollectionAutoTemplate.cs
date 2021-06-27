using ResourceManager;
using ResourceManager.Wpf.Converters;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace ResourceManager.Wpf.Controls
{

    public class DataCollectionAutoTemplate<T> : AutoTemplate<IList<T>>
    {
        public override DependencyObject LoadContent(object context, TemplateOptions options)
        {
            ListDataCard listCard = new ListDataCard();
            listCard.SetBinding(ListDataCard.HeaderTextProperty, new Binding("ItemsSource.Count") {
                StringFormat = "({0}) " + options.BindingName.MakeFriendlyName(),
                Source = listCard
            });
            PropertyInfo property = context.GetType().GetProperty(options.BindingName, BindingFlags.Public | BindingFlags.Instance);
            Type elementType = property.PropertyType.GetEnumerableElementType();
            listCard.SetBinding(ListDataCard.ItemsSourceProperty, new Binding(options.BindingName) {
                Converter = new ObservableListConverter(),
            });
            listCard.ItemContentTemplate = ResolveItemContentTemplate(context, options);
            return listCard;
        }

        protected virtual DataTemplate ResolveItemContentTemplate(object context, TemplateOptions options) => TemplateGenerator.CreateDataTemplate(() => new AutoControl() { PropertyName = "." });

    }
}
