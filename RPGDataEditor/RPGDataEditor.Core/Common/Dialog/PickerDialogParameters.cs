using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core
{
    public class PickerDialogParameters : DialogParametersBuilder
    {
        public PickerDialogParameters(object pickedItem) => PickedItem = pickedItem;

        public PickerDialogParameters(RPGResource pickResource) => PickResource = pickResource;

        public RPGResource PickResource {
            get => Get<RPGResource>();
            set => Set(value);
        }

        public object PickedItem {
            get => Get<object>();
            set => Set(value);
        }
    }
}
