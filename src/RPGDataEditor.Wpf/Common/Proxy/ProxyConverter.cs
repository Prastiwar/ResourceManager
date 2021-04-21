using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Converters
{
    public class ProxyConverter<T> : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<Proxy<T>> collection = new ObservableCollection<Proxy<T>>();
            if (value is Collection<T> src)
            {
                for (int i = 0; i < src.Count(); i++)
                {
                    collection.Add(new Proxy<T>(i, src));
                }
            }
            return collection;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<Proxy<T>> collection)
            {
                ObservableCollection<T> rawCollection = new ObservableCollection<T>();
                rawCollection.AddRange(collection.Select(x => x.Value));
                return rawCollection;
            }
            return null;
        }
    }
}
