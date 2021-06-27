using System.Windows;
using System.Windows.Controls;

namespace ResourceManager.Wpf.Views
{
    public partial class PickerDialog : UserControl
    {
        public PickerDialog() => InitializeComponent();

        private void ItemListView_Loaded(object sender, RoutedEventArgs e)
        {
            object selectedItem = ItemListView.SelectedItem;
            DependencyObject obj = ItemListView.ItemContainerGenerator.ContainerFromItem(selectedItem);
            if (obj is ListBoxItem item)
            {
                item.Focus();
            }
        }
    }
}
