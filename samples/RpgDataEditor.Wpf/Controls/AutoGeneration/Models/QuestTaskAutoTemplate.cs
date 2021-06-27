using RpgDataEditor.Models;
using ResourceManager.Wpf;
using ResourceManager.Wpf.Controls;
using System.Windows;

namespace RpgDataEditor.Wpf.Controls
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
