using ResourceManager;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Converters
{
    public class ObservableListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            if (value is INotifyCollectionChanged)
            {
                return value;
            }
            Type type = typeof(ObservableListProxy<>).MakeGenericType(value.GetType().GetEnumerableElementType());
            return Activator.CreateInstance(type, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !typeof(ObservableListProxy<>).IsAssignableFrom(value.GetType()))
            {
                return value;
            }
            Type type = typeof(ObservableListProxy<>).MakeGenericType(value.GetType().GetEnumerableElementType());
            PropertyInfo prop = type.GetProperty(nameof(ObservableListProxy<int>.InternalList), BindingFlags.Public | BindingFlags.Instance);
            return prop.GetValue(value);
        }
    }
}
