using Prism;
using Prism.Ioc;
using Prism.Services.Dialogs;
using RPGDataEditor.Core;
using RPGDataEditor.Core.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Views
{
    public partial class ItemPicker : UserControl
    {
        public ItemPicker()
        {
            InitializeComponent();
            DataContextChanged += ItemPicker_DataContextChanged;
        }

        public static DependencyProperty ResourceProperty = DependencyProperty.Register(nameof(Resource), typeof(RPGResource?), typeof(ItemPicker), new PropertyMetadata(null, OnResourceChanged));
        private static void OnResourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ItemPicker).OnResourceChanged((RPGResource?)e.OldValue, (RPGResource?)e.NewValue);
        public RPGResource? Resource {
            get => (RPGResource?)GetValue(ResourceProperty);
            set => SetValue(ResourceProperty, value);
        }

        public static DependencyProperty PickedIdProperty = DependencyProperty.Register(nameof(PickedId), typeof(int), typeof(ItemPicker), new PropertyMetadata(-1, OnPickedIdChanged));
        private static void OnPickedIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ItemPicker).OnPickedIdChanged((int)e.OldValue, (int)e.NewValue);
        public int PickedId {
            get => (int)GetValue(PickedIdProperty);
            set => SetValue(PickedIdProperty, value);
        }

        public static DependencyProperty PickedItemProperty = DependencyProperty.Register(nameof(PickedItem), typeof(IIdentifiable), typeof(ItemPicker), new PropertyMetadata(null, OnPickedItemChanged));
        private static void OnPickedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ItemPicker).OnPickedItemChanged(e.OldValue, e.NewValue);
        public IIdentifiable PickedItem {
            get => (IIdentifiable)GetValue(PickedItemProperty);
            set => SetValue(PickedItemProperty, value);
        }

        public static DependencyProperty EmptyTextProperty = DependencyProperty.Register(nameof(EmptyText), typeof(string), typeof(ItemPicker), new PropertyMetadata("None", OnEmptyTextChanged));
        private static void OnEmptyTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ItemPicker picker = (d as ItemPicker);
            if (picker.PickedItem == null)
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

        protected virtual async void OnPickedIdChanged(int oldId, int newId)
        {
            PickedItem = null;
            await ReassignItemAsync();
        }

        protected virtual async void OnResourceChanged(RPGResource? oldValue, RPGResource? newValue)
        {
            PickedItem = null;
            await ReassignItemAsync();
        }

        protected async Task ReassignItemAsync()
        {
            int id = PickedId;
            if (Resource == null || id == -1)
            {
                return;
            }
            if (PickedItem != null)
            {
                OnPickedItemChanged(PickedItem, PickedItem);
                return;
            }
            LoadingOverlay.Visibility = Visibility.Visible;
            ItemTextBlock.Text = "Loading...";
            string[] locations = await RpgDataEditorApplication.Current.Session.Client.GetAllLocationsAsync((int)Resource);
            ILocationToSimpleResourceConverter converter = RpgDataEditorApplication.Current.Container.Resolve<ILocationToSimpleResourceConverter>();
            SimpleIdentifiableData pickedItem = locations.Select(loc => converter.CreateSimpleData(loc)).FirstOrDefault(data => data.Id == id);
            PickedItem = pickedItem;
            LoadingOverlay.Visibility = Visibility.Collapsed;
        }

        private async void PickItem(object sender, RoutedEventArgs e) => await PickItemAsync();

        protected virtual async Task PickItemAsync()
        {
            IDialogService service = DialogService ?? (Application.Current as PrismApplicationBase).Container.Resolve<IDialogService>();
            IDialogResult result = await service.ShowDialogAsync("PickerDialog", new PickerDialogParameters(Resource.Value, PickedItem, PickedId).Build()).ConfigureAwait(true);
            if (result.Result == ButtonResult.OK)
            {
                IIdentifiable pickedItem = result.Parameters.GetValue<IIdentifiable>(nameof(PickerDialogParameters.PickedItem));
                PickedItem = pickedItem;
            }
        }

        private void ItemPicker_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) => OnPickedItemChanged(null, PickedItem);

        protected virtual void OnPickedItemChanged(object oldValue, object newValue)
        {
            if (PickedItem == null)
            {
                ItemTextBlock.Text = EmptyText;
            }
            else
            {
                ItemTextBlock.Text = PickedItem.ToString();
                if (PickedItem is IIdentifiable identifiable)
                {
                    PickedId = (int)identifiable.Id;
                }
            }
        }
    }
}
