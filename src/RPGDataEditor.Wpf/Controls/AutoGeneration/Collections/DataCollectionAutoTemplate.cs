using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Controls
{
    public class DataCollectionAutoTemplate<T> : AutoTemplate<IList<T>>
    {
        public override DependencyObject LoadContent(object context, TemplateOptions options)
        {
            ListDataCard listCard = new ListDataCard();
            listCard.SetBinding(ListDataCard.HeaderTextProperty, new Binding(options.BindingName + ".Count") { StringFormat = "({0}) " + options.BindingName.MakeFriendlyName() });
            listCard.SetBinding(ListDataCard.ItemsSourceProperty, new Binding(options.BindingName));
            listCard.ItemContentTemplate = ResolveItemContentTemplate(context, options);
            return listCard;
        }

        protected virtual DataTemplate ResolveItemContentTemplate(object context, TemplateOptions options) => TemplateGenerator.CreateDataTemplate(() => new AutoControl() { PropertyName = "." });
    }
}
