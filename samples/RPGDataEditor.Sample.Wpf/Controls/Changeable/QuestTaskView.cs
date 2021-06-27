using RPGDataEditor.Sample.Models;
using RPGDataEditor.Wpf.Controls;
using System.Windows;

namespace RPGDataEditor.Sample.Wpf.Controls
{
    public class QuestTaskView : ChangeableUserControl
    {
        private static readonly TypeSource[] sources = new TypeSource[] {
            new TypeSource("EntityInteract", typeof(EntityInteractQuestTask)),
            new TypeSource("Kill", typeof(KillQuestTask)),
            new TypeSource("Reach", typeof(ReachQuestTask)),
            new TypeSource("Dialogue", typeof(DialogueQuestTask))
        };

        protected override void OnTemplateApplied()
        {
            if (GetBindingExpression(TypesSourceProperty) == null)
            {
                TypesSource = GetSources();
            }
            base.OnTemplateApplied();
        }

        protected virtual TypeSource[] GetSources() => sources;

        protected override object GetActualContentResource(TypeSource type) => Application.Current.TryFindResource(type.Name + "QuestTaskContent");

        protected override TypeSource GetDataContextTypeSource()
        {
            if (DataContext == null)
            {
                return null;
            }
            return new TypeSource(DataContext.GetType().Name.Replace("QuestTask", ""), DataContext.GetType());
        }
    }
}
