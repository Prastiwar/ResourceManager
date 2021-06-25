using RPGDataEditor.Mvvm.Navigation;
using System;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Mvvm
{
    public class PickerDialogParameters : DialogParametersBuilder
    {
        public PickerDialogParameters(object pickedItem) => PickedItem = pickedItem;

        public PickerDialogParameters(Type resourceType, object pickedItem, object pickedId, IValueConverter modelNameConverter = null)
        {
            ResourceType = resourceType;
            PickedItem = pickedItem;
            PickedId = pickedId;
            ModelNameConverter = modelNameConverter;
        }

        public Type ResourceType {
            get => Get<Type>();
            set => Set(value);
        }

        public object PickedItem {
            get => Get<object>();
            set => Set(value);
        }

        public object PickedId {
            get => Get<object>();
            set => Set(value);
        }

        public IValueConverter ModelNameConverter {
            get => Get<IValueConverter>();
            set => Set(value);
        }
    }
}
