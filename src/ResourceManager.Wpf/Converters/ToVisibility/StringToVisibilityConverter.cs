using System;
using System.Globalization;
using System.Windows;

namespace ResourceManager.Wpf.Converters
{
    /// <summary> Compares object value with parameter value as strings. Returns Visibility.Visible if they are equal </summary>
    public class StringToVisibilityConverter : SimpleInvertableConverter<Visibility>
    {
        public StringToVisibilityConverter()
        {
            PositiveValue = Visibility.Visible;
            NegativeValue = Visibility.Collapsed;
        }

        public bool IgnoreCase { get; set; }

        public override Visibility ConvertTo(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string valueString = value == null ? "null" : value.ToString();
            bool equal = !IgnoreCase ? valueString == parameter.ToString() : (string.Compare(valueString, parameter.ToString(), true) == 0);
            return equal ? PositiveValue : NegativeValue;
        }

        public override object ConvertToBack(Visibility value, Type targetType, object parameter, CultureInfo culture)
            => throw ConverterExceptionMessages.GetNotSupportedConversion(typeof(Visibility), typeof(string));
    }
}
