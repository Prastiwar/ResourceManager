using Prism.Services.Dialogs;
using RPGDataEditor.Core;
using RPGDataEditor.Core.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Controls
{
    public class ResourcePicker : UserControl
    {
        public static DependencyProperty ResourceProperty = DependencyProperty.Register(nameof(Resource), typeof(RPGResource?), typeof(ResourcePicker), new PropertyMetadata(null, OnResourceChanged));
        private static void OnResourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ResourcePicker).OnResourceChanged((RPGResource?)e.OldValue, (RPGResource?)e.NewValue);
        public RPGResource? Resource {
            get => (RPGResource?)GetValue(ResourceProperty);
            set => SetValue(ResourceProperty, value);
        }

        public static DependencyProperty PickedIdProperty = DependencyProperty.Register(nameof(PickedId), typeof(int), typeof(ResourcePicker), new PropertyMetadata(-1, OnPickedIdChanged));
        private static void OnPickedIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ResourcePicker).OnPickedIdChanged((int)e.OldValue, (int)e.NewValue);
        public int PickedId {
            get => (int)GetValue(PickedIdProperty);
            set => SetValue(PickedIdProperty, value);
        }

        public static DependencyProperty PickedItemProperty = DependencyProperty.Register(nameof(PickedItem), typeof(IIdentifiable), typeof(ResourcePicker), new PropertyMetadata(null, OnPickedItemChanged));
        private static void OnPickedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ResourcePicker).OnPickedItemChanged(e.OldValue, e.NewValue);
        public IIdentifiable PickedItem {
            get => (IIdentifiable)GetValue(PickedItemProperty);
            set => SetValue(PickedItemProperty, value);
        }

        public static DependencyProperty EmptyTextProperty = DependencyProperty.Register(nameof(EmptyText), typeof(string), typeof(ResourcePicker), new PropertyMetadata("None", OnEmptyTextChanged));
        private static void OnEmptyTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ResourcePicker picker = (d as ResourcePicker);
            if (picker.PickedItem == null)
            {
                if (picker.resourceTextBlock != null)
                {
                    picker.resourceTextBlock.Text = e.NewValue as string;
                }
            }
        }

        public string EmptyText {
            get => (string)GetValue(EmptyTextProperty);
            set => SetValue(EmptyTextProperty, value);
        }

        private TextBlock resourceTextBlock;
        private UIElement loadingOverlay;
        private Button pickButton;

        private bool isLoading;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            resourceTextBlock = Template.FindName("ResourceTextBlock", this) as TextBlock;
            loadingOverlay = Template.FindName("LoadingOverlay", this) as UIElement;
            pickButton = Template.FindName("PickButton", this) as Button;

            pickButton.Click += PickButton_Click;
            DataContextChanged += ResourcePicker_DataContextChanged;

            resourceTextBlock.Text = isLoading ? "Loading..." : PickedItem == null ? EmptyText : PickedItem.ToString();
            loadingOverlay.Visibility = isLoading ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ResourcePicker_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) => OnPickedItemChanged(null, PickedItem);

        protected virtual void OnPickedItemChanged(object oldValue, object newValue)
        {
            if (PickedItem == null)
            {
                if (resourceTextBlock != null)
                {
                    resourceTextBlock.Text = EmptyText;
                }
            }
            else
            {
                if (resourceTextBlock != null)
                {
                    resourceTextBlock.Text = PickedItem.ToString();
                }
                if (PickedItem is IIdentifiable identifiable)
                {
                    PickedId = (int)identifiable.Id;
                }
            }
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

        protected virtual async Task ReassignItemAsync()
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
            isLoading = true;
            if (loadingOverlay != null)
            {
                loadingOverlay.Visibility = Visibility.Visible;
            }
            if (resourceTextBlock != null)
            {
                resourceTextBlock.Text = "Loading...";
            }
            try
            {
                string[] locations = await RpgDataEditorApplication.Current.Session.Client.GetAllLocationsAsync((int)Resource);
                ILocationToSimpleResourceConverter converter = Application.Current.TryResolve<ILocationToSimpleResourceConverter>();
                SimpleIdentifiableData pickedItem = locations.Select(loc => converter.CreateSimpleData(loc)).FirstOrDefault(data => data.Id == id);
                PickedItem = pickedItem;
            }
            catch (System.Exception ex)
            {
                resourceTextBlock.Text = "Failed to load";
                Logger.Error("Couldn't load locations", ex);
            }
            isLoading = false;
            if (loadingOverlay != null)
            {
                loadingOverlay.Visibility = Visibility.Collapsed;
            }
        }

        private async void PickButton_Click(object sender, RoutedEventArgs e) => await PickItemAsync();

        protected virtual async Task PickItemAsync()
        {
            IDialogService service = Application.Current.TryResolve<IDialogService>();
            IDialogResult result = await service.ShowDialogAsync("PickerDialog", new PickerDialogParameters(Resource.Value, PickedItem, PickedId).Build()).ConfigureAwait(true);
            if (result.Result == ButtonResult.OK)
            {
                IIdentifiable pickedItem = result.Parameters.GetValue<IIdentifiable>(nameof(PickerDialogParameters.PickedItem));
                PickedItem = pickedItem;
            }
        }
    }
}
