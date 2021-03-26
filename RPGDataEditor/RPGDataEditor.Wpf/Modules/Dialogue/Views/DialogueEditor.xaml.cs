using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Dialogue.Views
{
    public partial class DialogueEditor : UserControl
    {
        public DialogueEditor() => InitializeComponent();

        private void RequirementView_TypeChange(object sender, Controls.ChangeableUserControl.ChangeTypeEventArgs e)
        {
            if (DataContext is ModelDialogViewModel<DialogueModel> vm)
            {
                e.ChangeTypeInList(vm.Model.Requirements, RequirementsListView);
            }
        }
    }
}
