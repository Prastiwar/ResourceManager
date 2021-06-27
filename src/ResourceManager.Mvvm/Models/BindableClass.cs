using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ResourceManager.Mvvm
{
    public class BindableClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected virtual bool SetProperty<T>(ref T storage, T value, bool shouldAlwaysNotify, [CallerMemberName] string propertyName = null)
        {
            bool isSet = SetProperty(ref storage, value, propertyName);
            if (!isSet && shouldAlwaysNotify)
            {
                OnPropertyChanged(propertyName);
                return true;
            }
            return isSet;
        }

        protected virtual bool SetProperty<T>(T property, T value, Action set, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(property, value))
            {
                return false;
            }
            set?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args) => PropertyChanged?.Invoke(this, args);
    }
}
