using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Npc.Views
{
    public partial class NpcDataModelEditor : UserControl
    {
        public NpcDataModelEditor() => InitializeComponent();

        private void ToggleExpandIcon(object sender, System.Windows.RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Commands.ToggleExpandIcon(btn);
        }

        private void JobComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                if (e.AddedItems[0] is ComboBoxItem selected)
                {
                    bool isGuard = string.Compare(selected.Name, "guard", true) == 0;
                    bool isTrader = string.Compare(selected.Name, "trader", true) == 0;
                    TraderStackPanel.Visibility = isTrader ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
                    GuardStackPanel.Visibility = isGuard ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
                    // TODO: Set job to model
                }
            }
        }
    }
}
