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

        public RPGResource Resource { get; set; }

        public static DependencyProperty PickedIdProperty = DependencyProperty.Register(nameof(PickedId), typeof(int), typeof(ItemPicker), new PropertyMetadata(-1));
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

        protected override async void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == PickedIdProperty)
            {
                await ReassignItemAsync();
            }
        }

        protected async Task ReassignItemAsync()
        {
            int id = PickedId;
            switch (Resource)
            {
                case RPGResource.Quest:
                    QuestModel[] quests = await App.CurrentSession.LoadQuests();
                    PickedItem = quests.FirstOrDefault(q => q.Id == id);
                    break;
                case RPGResource.Dialogue:
                    DialogueModel[] dialogues = await App.CurrentSession.LoadDialogues();
                    PickedItem = dialogues.FirstOrDefault(d => d.Id == id);
                    break;
                case RPGResource.Npc:
                    NpcDataModel[] npcs = await App.CurrentSession.LoadNpcs();
                    PickedItem = npcs.FirstOrDefault(d => d.Id == id);
                    break;
                default:
                    break;
            }
        }

        private async void PickItem(object sender, RoutedEventArgs e) => await PickItemAsync();

        protected virtual async Task PickItemAsync()
        {
            IDialogService service = DialogService ?? (Application.Current as PrismApplicationBase).Container.Resolve<IDialogService>();
            IDialogResult result = await service.ShowDialogAsync("PickerDialog", new PickerDialogParameters(Resource, PickedItem, PickedId).Build()).ConfigureAwait(true);
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
                    PickedId = identifiable.Id;
                }
            }
        }
    }
}
