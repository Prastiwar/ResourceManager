using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Converters
{
    public class NextDialogIdToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is int id ? id >= 0 ? Visibility.Visible : Visibility.Collapsed: Visibility.Collapsed;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException("Conversion from Visibility to id is not supported");
    }
}
