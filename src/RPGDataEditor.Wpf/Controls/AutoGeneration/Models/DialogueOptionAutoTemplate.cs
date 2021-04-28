using RPGDataEditor.Extensions.Prism.Wpf;
using RPGDataEditor.Models;
using System.Reflection;
using System.Windows;

namespace RPGDataEditor.Wpf.Controls
{
    public class DialogueOptionAutoTemplate : AutoTemplate<DialogueOption>
    {
        public override DependencyObject LoadContent(PropertyInfo info = null)
        {
            DialogueOptionView view = new DialogueOptionView();
            AutoControl.SetPreserveDataContext(view, false);
            view.TypeChange += View_TypeChange;
            return view;
        }

        private void View_TypeChange(object sender, ChangeableUserControl.ChangeTypeEventArgs e)
        {
            DialogueOption targetModel = (DialogueOption)e.Item;
            int? id = Application.Current.TryResolve<INamedIdProvider<DialogueOption>>()?.GetId(e.TargetType);
            if (id.HasValue)
            {
                targetModel.NextDialogId = id.Value;
            }
        }
    }
}
