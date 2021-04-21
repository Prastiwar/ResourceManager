using RPGDataEditor.Core;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Controls
{
    public class DataCollectionAutoTemplate<T> : AutoTemplate<IList<T>>
    {
        public override DependencyObject LoadContent(PropertyInfo info)
        {
            ListDataCard listCard = new ListDataCard();
            listCard.SetBinding(ListDataCard.HeaderTextProperty, new Binding(info.Name + ".Count") { StringFormat = "({0}) " + info.Name.MakeFriendlyName() });
            listCard.SetBinding(ListDataCard.ItemsSourceProperty, new Binding(info.Name));
            listCard.ItemContentTemplate = ResolveItemContentTemplate(info);
            return listCard;
        }

        protected virtual DataTemplate ResolveItemContentTemplate(PropertyInfo info) => TemplateGenerator.CreateDataTemplate(() => new AutoControl() { PropertyName = "." });
    }
}
