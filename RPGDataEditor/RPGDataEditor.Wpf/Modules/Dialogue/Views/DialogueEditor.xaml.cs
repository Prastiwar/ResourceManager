using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Providers;
using System.Windows;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Dialogue.Views
{
    public partial class DialogueEditor : UserControl
    {
        public DialogueEditor() => InitializeComponent();

        private void DialogueOptionView_TypeChange(object sender, Controls.ChangeableUserControl.ChangeTypeEventArgs e)
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
