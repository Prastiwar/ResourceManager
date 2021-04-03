using System;
using System.Globalization;

namespace RPGDataEditor.Wpf.Converters
{
    /// <summary> Compares object value with parameter value as strings. Returns True if they are equal </summary>
    public class StringToBoolConverter : SimpleInvertableConverter<bool>
    {
        public StringToBoolConverter() => PositiveValue = true;

        public bool IgnoreCase { get; set; }

        public override bool ConvertTo(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string valueString = value == null ? "null" : value.ToString();
            return !IgnoreCase ? valueString == parameter.ToString() : (string.Compare(valueString, parameter.ToString(), true) == 0);
        }
        public override object ConvertToBack(bool value, Type targetType, object parameter, CultureInfo culture)
            => throw ConverterExceptionMessages.GetNotSupportedConversion(typeof(bool), typeof(string));
    }
}
