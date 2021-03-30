using Prism;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Providers;
using System.Windows;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Controls
{
    public class DialogueOptionView : ChangeableUserControl
    {
        private static readonly string[] types = new string[] {
            "Quit",
            "Job",
            "Dialogue"
        };
        private ContentPresenter actualContent;

        protected override void OnTemplateApplied()
        {
            if (GetBindingExpression(TypesSourceProperty) == null)
            {
                TypesSource = GetOptionNames();
            }
            if (Template != null)
            {
                actualContent = Template.FindName("ActualContent", this) as ContentPresenter;
            }
            base.OnTemplateApplied();
        }

        protected virtual string[] GetOptionNames() => types;

        protected override object GetActualContentResource(string name) => actualContent.Content ?? Application.Current.TryFindResource("DialogueOptionContent");

        protected override string GetDataContextItemName()
        {
            if (DataContext is DialogueOptionModel model)
            {
                return Application.Current.TryResolve<INamedIdProvider<DialogueOptionModel>>()?.GetName(model.NextDialogId);
            }
            return null;
        }
    }
}
