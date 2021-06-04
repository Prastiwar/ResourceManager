using RPGDataEditor.Models;
using System.Windows;

namespace RPGDataEditor.Wpf.Controls
{
    public class DialogueOptionAutoTemplate : AutoTemplate<DialogueOption>
    {
        public override DependencyObject LoadContent(object context, TemplateOptions options)
        {
            DialogueOptionView view = new DialogueOptionView();
            AutoControl.SetPreserveDataContext(view, false);
            view.TypeChange += View_TypeChange;
            return view;
        }

        private void View_TypeChange(object sender, ChangeableUserControl.ChangeTypeEventArgs e)
        {
            DialogueOption targetModel = (DialogueOption)e.Item;
            string optionName = e.TargetType.Name;
            if (string.Compare(optionName, "quit", true) == 0)
            {
                targetModel.NextDialogId = -1;
            }
            else if (string.Compare(optionName, "job", true) == 0)
            {
                targetModel.NextDialogId = -2;
            }
            else
            {
                targetModel.NextDialogId = 0;
            }
        }
    }
}
