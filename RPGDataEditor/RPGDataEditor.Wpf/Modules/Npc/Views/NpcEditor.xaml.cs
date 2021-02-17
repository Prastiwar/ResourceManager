using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Npc.Views
{
    public partial class NpcEditor : UserControl
    {
        public NpcEditor() => InitializeComponent();

        private void ToggleExpandIcon(object sender, System.Windows.RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Commands.ToggleExpandIcon(btn);
        }
    }
}
