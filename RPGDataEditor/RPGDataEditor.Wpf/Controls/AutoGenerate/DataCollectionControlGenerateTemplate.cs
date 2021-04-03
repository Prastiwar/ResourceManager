using RPGDataEditor.Core;
using RPGDataEditor.Core.Models;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Controls
{
    public class RequirementDataCollectionControlGenerateTemplate : ControlGenerateTemplate<IList<PlayerRequirementModel>>
    {
        public RequirementDataCollectionControlGenerateTemplate(PropertyInfo info) : base(info) { }

        public override DependencyObject LoadContent()
        {
            ListDataCard listCard = new ListDataCard();
            listCard.SetBinding(FrameworkElement.DataContextProperty, new Binding("Model"));
            listCard.SetResourceReference(FrameworkElement.StyleProperty, "RequirementsListCardStyle");
            return listCard;
        }
    }

    public class DataCollectionControlGenerateTemplate<T> : ControlGenerateTemplate<IList<T>>
    {
        public DataCollectionControlGenerateTemplate(PropertyInfo info) : base(info) { }

        public override DependencyObject LoadContent()
        {
            ListDataCard listCard = new ListDataCard();
            listCard.SetBinding(FrameworkElement.DataContextProperty, new Binding("Model"));
            listCard.SetBinding(ListDataCard.HeaderTextProperty, new Binding(Info.Name + ".Count") { StringFormat = "({0})" + Info.Name.MakeFriendlyName() });
            listCard.SetBinding(ListDataCard.ItemsSourceProperty, new Binding(Info.Name));
            // TODO: Resolve collection item type and find generator
            //listCard.ItemContentTemplate = TemplateGenerator.CreateDataTemplate(() => resolvedGenerator.LoadContent()); 
            return listCard;
        }
    }
}
