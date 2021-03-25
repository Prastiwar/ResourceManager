using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Converters
{
    public class ItemsSourceToVisibilityConverter : IValueConverter
    {
        public Visibility PositiveValue { get; set; } = Visibility.Visible;
        public Visibility EmptyValue { get; set; } = Visibility.Collapsed;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable enumrable)
            {
                return enumrable.Cast<object>().Any() ? PositiveValue : EmptyValue;
            }
            return EmptyValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility? visibility = value as Visibility?;
            return visibility == PositiveValue ? 1 : 0;
        }
    }
}
