using RPGDataEditor.Models;
using System.Windows;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Controls
{
    public class DialogueOptionView : ChangeableUserControl
    {
        private static readonly TypeSource[] sources = new TypeSource[] {
            new TypeSource("Quit", typeof(DialogueOption)),
            new TypeSource("Job", typeof(DialogueOption)),
            new TypeSource("Dialogue", typeof(DialogueOption))
        };

        private ContentPresenter actualContent;

        protected override void OnTemplateApplied()
        {
            if (GetBindingExpression(TypesSourceProperty) == null)
            {
                TypesSource = GetSources();
            }
            if (Template != null)
            {
                actualContent = Template.FindName("ActualContent", this) as ContentPresenter;
            }
            base.OnTemplateApplied();
        }

        protected virtual TypeSource[] GetSources() => sources;

        protected override object GetActualContentResource(TypeSource type) => actualContent.Content ?? Application.Current.TryFindResource("DialogueOptionContent");

        protected override TypeSource GetDataContextTypeSource()
        {
            if (DataContext is DialogueOption model)
            {
                return model.NextDialogId switch {
                    -1 => sources[0],
                    -2 => sources[1],
                    0 => null,
                    _ => sources[2],
                };
            }
            return null;
        }
    }
}
