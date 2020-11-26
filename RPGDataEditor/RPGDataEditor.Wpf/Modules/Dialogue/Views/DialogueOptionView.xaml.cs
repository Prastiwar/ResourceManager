using RPGDataEditor.Core.Models;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Dialogue.Views
{
    public partial class DialogueOptionView : UserControl
    {
        public DialogueOptionView() => InitializeComponent();

        private void OptionType_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                if (e.AddedItems[0] is ComboBoxItem selected)
                {
                    bool isDialogue = string.Compare(selected.Name, "dialogue", true) == 0;
                    bool isJob = string.Compare(selected.Name, "TriggerJob", true) == 0;
                    bool isQuit = string.Compare(selected.Name, "quit", true) == 0;
                    DialogueOptionPanel.Visibility = isDialogue ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
                    if (DataContext is DialogueOptionModel model)
                    {
                        model.NextDialogId = isQuit ? -1 :
                                             isJob ? -2 :
                                             -1;
                    }
                }
            }
        }
    }
}
