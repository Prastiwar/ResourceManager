using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace ResourceManager.Wpf.Converters
{
    /// <summary> Converts IEnumerable collection to Visibility. Returns Visibility.Visible if it contains any element </summary>
    public class ItemsSourceToVisibilityConverter : SimpleInvertableConverter<Visibility>
    {
        public ItemsSourceToVisibilityConverter()
        {
            PositiveValue = Visibility.Visible;
            NegativeValue = Visibility.Collapsed;
        }

        public override Visibility ConvertTo(object value, Type targetType, object parameter, CultureInfo culture)
            => value is IEnumerable enumrable ? enumrable.Cast<object>().Any() ? PositiveValue : NegativeValue : NegativeValue;

        public override object ConvertToBack(Visibility value, Type targetType, object parameter, CultureInfo culture) 
            => throw ConverterExceptionMessages.GetNotSupportedConversion(typeof(Visibility), typeof(IEnumerable));
    }
}
