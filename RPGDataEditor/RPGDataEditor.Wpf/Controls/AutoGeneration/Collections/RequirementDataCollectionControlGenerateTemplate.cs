using RPGDataEditor.Core.Models;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Controls
{
    public class RequirementDataCollectionControlGenerateTemplate : DataCollectionControlGenerateTemplate<PlayerRequirementModel>
    {
        public override DependencyObject LoadContent(PropertyInfo info)
        {
            ListDataCard listCard = base.LoadContent(info) as ListDataCard;
            listCard.AddItemCommand = Commands.AddRequirementCommand;
            listCard.SetBinding(ListDataCard.AddItemCommandParameterProperty, new Binding(info.Name));
            return listCard;
        }

        protected override DataTemplate ResolveItemContentTemplate(PropertyInfo info) => (DataTemplate)Application.Current.FindResource("RequirementsListCardItemTemplate");
    }
}
