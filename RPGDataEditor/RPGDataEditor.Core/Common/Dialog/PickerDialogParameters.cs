using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core
{
    public class PickerDialogParameters : DialogParametersBuilder
    {
        public PickerDialogParameters(IIdentifiable pickedItem) => PickedItem = pickedItem;

        public PickerDialogParameters(RPGResource pickResource, IIdentifiable pickedItem = null, int pickedId = -1)
        {
            PickResource = pickResource;
            PickedItem = pickedItem;
            PickedId = pickedId;
        }

        public RPGResource PickResource {
            get => Get<RPGResource>();
            set => Set(value);
        }

        public IIdentifiable PickedItem {
            get => Get<IIdentifiable>();
            set => Set(value);
        }

        public int PickedId {
            get => Get<int>();
            set => Set(value);
        }
    }
}
