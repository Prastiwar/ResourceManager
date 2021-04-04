using RPGDataEditor.Core.Models;
using System.Reflection;
using System.Windows;

namespace RPGDataEditor.Wpf.Controls
{
    public class QuestTaskAutoTemplate : AutoTemplate<QuestTask>
    {
        public override DependencyObject LoadContent(PropertyInfo info = null)
        {
            QuestTaskView view = new QuestTaskView();
            AutoControl.SetPreserveDataContext(view, false);
            view.TypeChange += View_TypeChange;
            return view;
        }

        private void View_TypeChange(object sender, ChangeableUserControl.ChangeTypeEventArgs e) => e.ChangeType<QuestTask>(sender);
    }
}
