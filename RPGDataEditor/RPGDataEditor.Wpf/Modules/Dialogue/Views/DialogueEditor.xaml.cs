using MaterialDesignThemes.Wpf;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Dialogue.Views
{
    public partial class DialogueEditor : UserControl
    {
        public DialogueEditor() => InitializeComponent();

        private readonly PackIcon lessIcon = new PackIcon() { Kind = PackIconKind.ExpandLess, FontSize = 14 };
        private readonly PackIcon moreIcon = new PackIcon() { Kind = PackIconKind.ExpandMore, FontSize = 14 };

        private void ToggleExpandIcon(object sender, System.Windows.RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Content is PackIcon icon)
            {
                icon.Kind = icon.Kind == PackIconKind.ExpandMore ? PackIconKind.ExpandLess : PackIconKind.ExpandMore;
            }
        }
    }
}
