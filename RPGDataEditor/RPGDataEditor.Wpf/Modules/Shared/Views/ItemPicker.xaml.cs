using Prism.Services.Dialogs;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace RPGDataEditor.Wpf.Views
{
    public partial class ItemPicker : Selector
    {
        public ItemPicker()
        {
            InitializeComponent();
            SelectedValue = "None";
        }

        internal IDialogService DialogService { get; set; }

        private void PickItem(object sender, RoutedEventArgs e)
        {
            DialogService.ShowDialog("ItemWindow", null, null);
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            ItemTextBlock.Text = SelectedValue.ToString();
        }
    }
}
