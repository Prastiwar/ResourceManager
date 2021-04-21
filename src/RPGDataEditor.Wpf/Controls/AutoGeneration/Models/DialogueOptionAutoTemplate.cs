using RPGDataEditor.Core;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Providers;
using System.Reflection;
using System.Windows;

namespace RPGDataEditor.Wpf.Controls
{
    public class DialogueOptionAutoTemplate : AutoTemplate<DialogueOptionModel>
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
            DialogueOptionModel targetModel = (DialogueOptionModel)e.Item;
            int? id = Application.Current.TryResolve<INamedIdProvider<DialogueOptionModel>>()?.GetId(e.TargetType);
            if (id.HasValue)
            {
                targetModel.NextDialogId = id.Value;
            }
        }
    }
}
