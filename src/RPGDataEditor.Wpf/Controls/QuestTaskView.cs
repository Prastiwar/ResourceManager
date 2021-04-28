using RPGDataEditor.Models;
using System;
using System.Windows;

namespace RPGDataEditor.Wpf.Controls
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

        protected override object GetActualContentResource(Type type) => Application.Current.TryFindResource(type.Name + "QuestTaskContent");

        protected override Type GetDataContextItemType() => DataContext?.GetType();
    }
}
