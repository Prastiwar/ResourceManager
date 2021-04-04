using RPGDataEditor.Core.Models;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Controls.AutoGeneration.Models
{
    public class RequirementAutoTemplate : AutoTemplate<PlayerRequirementModel>
    {
        public override DependencyObject LoadContent(PropertyInfo info = null)
        {
            RequirementView view = new RequirementView();
            view.TypeChange += View_TypeChange;
            view.SetBinding(AttachProperties.DataProperty, new Binding("ItemsSource") {
                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(ListDataCard), 1)
            });
            return view;
        }

        private void View_TypeChange(object sender, ChangeableUserControl.ChangeTypeEventArgs e)
        {
            if (AttachProperties.GetData(sender as UIElement) is IList<PlayerRequirementModel> list)
            {
                e.ChangeTypeInList(list);
            }
        }
    }
}
