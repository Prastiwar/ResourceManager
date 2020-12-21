using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Dialogue.Views
{
    public partial class DialogueEditor : UserControl
    {
        public DialogueEditor() => InitializeComponent();

        private void ToggleExpandIcon(object sender, System.Windows.RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Commands.ToggleExpandIcon(btn);
        }
    }
}
