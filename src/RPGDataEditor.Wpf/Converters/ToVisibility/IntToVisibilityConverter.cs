using System;
using System.Globalization;
using System.Windows;

namespace RPGDataEditor.Wpf.Converters
{
    /// <summary> Returns Visibility.Visible if value is positive integer. Converts back to 1 or 0 </summary>
    public class IntToVisibilityConverter : SimpleInvertableConverter<Visibility>
    {
        public IntToVisibilityConverter()
        {
            PositiveValue = Visibility.Visible;
            NegativeValue = Visibility.Collapsed;
        }

        public override Visibility ConvertTo(object value, Type targetType, object parameter, CultureInfo culture) => value is int num ? num > 0 ? PositiveValue : NegativeValue : NegativeValue;

        public override object ConvertToBack(Visibility value, Type targetType, object parameter, CultureInfo culture) => value == PositiveValue ? 1 : 0;
    }
}
