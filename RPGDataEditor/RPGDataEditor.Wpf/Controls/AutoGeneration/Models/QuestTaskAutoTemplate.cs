using RPGDataEditor.Core.Models;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Controls
{
    public class QuestTaskAutoTemplate : AutoTemplate<QuestTask>
    {
        public override DependencyObject LoadContent(PropertyInfo info = null)
        {
            QuestTaskView view = new QuestTaskView();
            view.TypeChange += View_TypeChange;
            view.SetBinding(AttachProperties.DataProperty, new Binding("ItemsSource") {
                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(ListDataCard), 1)
            });
            return view;
        }

        private void View_TypeChange(object sender, ChangeableUserControl.ChangeTypeEventArgs e)
        {
            if (AttachProperties.GetData(sender as UIElement) is IList<QuestTask> list)
            {
                e.ChangeTypeInList(list);
            }
        }
    }
}
