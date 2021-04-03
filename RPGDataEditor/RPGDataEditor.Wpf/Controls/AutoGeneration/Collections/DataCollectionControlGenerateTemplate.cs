using RPGDataEditor.Core;
using RPGDataEditor.Wpf.Providers;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Controls
{
    public class DataCollectionControlGenerateTemplate<T> : ControlGenerateTemplate<IList<T>>
    {
        public override DependencyObject LoadContent(PropertyInfo info)
        {
            ListDataCard listCard = new ListDataCard();
            listCard.SetBinding(ListDataCard.HeaderTextProperty, new Binding(info.Name + ".Count") { StringFormat = "({0}) " + info.Name.MakeFriendlyName() });
            listCard.SetBinding(ListDataCard.ItemsSourceProperty, new Binding(info.Name));
            listCard.ItemContentTemplate = ResolveItemContentTemplate(info);
            return listCard;
        }

        protected virtual DataTemplate ResolveItemContentTemplate(PropertyInfo info)
        {
            IControlGenerateTemplateProvider provider = Application.Current.TryResolve<IControlGenerateTemplateProvider>();
            ControlGenerateTemplate resolvedGenerator = provider.Resolve(info.PropertyType.GetElementType());
            // TODO: Create correct generic template
            //return TemplateGenerator.CreateDataTemplate(() => resolvedGenerator.LoadContent()); 
            return null;
        }
    }
}
