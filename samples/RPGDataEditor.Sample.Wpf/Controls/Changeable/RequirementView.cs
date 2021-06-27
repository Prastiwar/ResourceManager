using RPGDataEditor.Sample.Models;
using RPGDataEditor.Wpf.Controls;
using System.Windows;

namespace RPGDataEditor.Sample.Wpf.Controls
{
    public class RequirementView : ChangeableUserControl
    {
        private static readonly TypeSource[] sources = new TypeSource[] {
            new TypeSource("Dialogue", typeof(DialogueRequirement)),
            new TypeSource("Quest", typeof(QuestRequirement)),
            new TypeSource("Item", typeof(ItemRequirement))
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

        protected override object GetActualContentResource(TypeSource type) => Application.Current.TryFindResource(type.Name + "RequirementContent");

        protected override TypeSource GetDataContextTypeSource()
        {
            if (DataContext == null)
            {
                return null;
            }
            return new TypeSource(DataContext.GetType().Name.Replace("Requirement", ""), DataContext.GetType());
        }
    }
}
