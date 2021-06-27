using ResourceManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace RPGDataEditor.Wpf.Converters
{
    public class ObservableListProxy<T> : ObservableCollection<T>
    {
        public ObservableListProxy(IList<T> internalList) : base(internalList)
        {
            InternalList = internalList ?? throw new ArgumentNullException(nameof(internalList));
            CollectionChanged += ObservableList_CollectionChanged;
        }

        public IList<T> InternalList { get; }

        private void ObservableList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    InternalList.AddRange(e.NewItems.Cast<T>());
                    break;
                case NotifyCollectionChangedAction.Remove:
                    InternalList.RemoveRange(e.OldItems.Cast<T>());
                    break;
                case NotifyCollectionChangedAction.Reset:
                    InternalList.Clear();
                    break;
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Move:
                    throw new NotImplementedException("Replace and Move operations are not ready");
                default:
                    break;
            }
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(Count)));
        }
    }
}
