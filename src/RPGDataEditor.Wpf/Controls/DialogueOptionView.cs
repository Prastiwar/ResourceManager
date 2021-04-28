using RPGDataEditor.Extensions.Prism.Wpf;
using RPGDataEditor.Models;
using RPGDataEditor.Providers;
using System;
using System.Windows;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Controls
{
    public class DialogueOptionView : ChangeableUserControl
    {
        private static readonly TypeSource[] sources = new TypeSource[] {
            new TypeSource("Quit", ),
            new TypeSource("Job", ),
            new TypeSource("Dialogue", )
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

        protected override object GetActualContentResource(Type type) => actualContent.Content ?? Application.Current.TryFindResource("DialogueOptionContent");

        protected override Type GetDataContextItemType()
        {
            if (DataContext is DialogueOption model)
            {
                return Application.Current.TryResolve<INamedIdProvider<DialogueOption>>()?.GetName((int)model.NextDialogId);
            }
            return null;
        }
    }
}
