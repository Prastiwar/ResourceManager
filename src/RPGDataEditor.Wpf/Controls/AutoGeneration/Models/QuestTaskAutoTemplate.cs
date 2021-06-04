using RPGDataEditor.Models;
using System.Windows;

namespace RPGDataEditor.Wpf.Controls
{
    public class QuestTaskAutoTemplate : AutoTemplate<IQuestTask>
    {
        public override DependencyObject LoadContent(object context, TemplateOptions options)
        {
            QuestTaskView view = new QuestTaskView();
            AutoControl.SetPreserveDataContext(view, false);
            view.TypeChange += View_TypeChange;
            return view;
        }

        private void View_TypeChange(object sender, ChangeableUserControl.ChangeTypeEventArgs e) => e.ChangeType<IQuestTask>(sender);
    }
}
