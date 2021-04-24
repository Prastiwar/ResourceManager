using RPGDataEditor.Mvvm.Navigation;
using System;

namespace RPGDataEditor.Wpf.Mvvm
{
    public class PickerDialogParameters : DialogParametersBuilder
    {
        public PickerDialogParameters(IIdentifiable pickedItem) => PickedItem = pickedItem;

        public PickerDialogParameters(Type resourceType, IIdentifiable pickedItem = null, int pickedId = -1)
        {
            ResourceType = resourceType;
            PickedItem = pickedItem;
            PickedId = pickedId;
        }

        public Type ResourceType {
            get => Get<Type>();
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
