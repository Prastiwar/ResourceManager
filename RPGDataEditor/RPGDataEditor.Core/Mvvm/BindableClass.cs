using Prism.Mvvm;
using System.Runtime.CompilerServices;

namespace RPGDataEditor.Core.Mvvm
{
    public class BindableClass : BindableBase
    {
        protected virtual bool SetProperty<T>(ref T storage, T value, bool shouldAlwaysNotify, [CallerMemberName] string propertyName = null)
        {
            bool isSet = SetProperty(ref storage, value, propertyName);
            if (!isSet && shouldAlwaysNotify)
            {
                RaisePropertyChanged(propertyName);
                return true;
            }
            return isSet;
        }
    }
}
