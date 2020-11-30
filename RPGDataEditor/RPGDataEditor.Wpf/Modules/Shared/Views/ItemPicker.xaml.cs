using Prism;
using Prism.Ioc;
using Prism.Services.Dialogs;
using RPGDataEditor.Core;
using RPGDataEditor.Core.Models;
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
            DataContextChanged += ItemPicker_DataContextChanged;
        }

        public RPGResource Resource { get; set; }

        public static DependencyProperty EmptyTextProperty = DependencyProperty.Register(nameof(EmptyText), typeof(string), typeof(ItemPicker), new PropertyMetadata("None", OnEmptyTextChanged));
        private static void OnEmptyTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ItemPicker picker = (d as ItemPicker);
            if (picker.SelectedIndex == -1)
            {
                picker.ItemTextBlock.Text = e.NewValue as string;
            }
        }

        public string EmptyText {
            get => (string)GetValue(EmptyTextProperty);
            set => SetValue(EmptyTextProperty, value);
        }

        public static DependencyProperty DialogServiceProperty = DependencyProperty.Register(nameof(DialogService), typeof(IDialogService), typeof(ItemPicker));
        public IDialogService DialogService {
            get => (IDialogService)GetValue(DialogServiceProperty);
            set => SetValue(DialogServiceProperty, value);
        }

        private async void PickItem(object sender, RoutedEventArgs e)
        {
            IDialogService service = DialogService ?? (Application.Current as PrismApplicationBase).Container.Resolve<IDialogService>();
            IDialogResult result = await service.ShowDialogAsync("PickerDialog", new PickerDialogParameters(Resource).Build());
            if (result.Result == ButtonResult.OK)
            {
                object pickedItem = result.Parameters.GetValue<object>(nameof(PickerDialogParameters.PickedItem));
                SelectedItem = pickedItem;
            }
        }

        private void ItemPicker_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) => FillSelection();

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            FillSelection();
        }

        protected virtual void FillSelection()
        {
            if (SelectedIndex < 0)
            {
                ItemTextBlock.Text = EmptyText;
            }
            if (SelectedItem is IIdentifiable data)
            {
                ItemTextBlock.Text = SelectedItem.ToString();
            }
        }
    }
}
