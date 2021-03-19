using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Converters
{
    public class StringCompareToVisibilityConverter : IValueConverter
    {
        public Visibility PositiveValue { get; set; } = Visibility.Visible;
        public Visibility ZeroNegativeValue { get; set; } = Visibility.Collapsed;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool equal = value.ToString() == parameter.ToString();
            return equal ? PositiveValue : ZeroNegativeValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException("Convert visiblity to string comparison is not supported");
    }
}
