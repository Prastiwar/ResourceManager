using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Converters
{
    /// <summary> Converts IEnumerable collection to bool. Returns True if it contains any element </summary>
    public class ItemsSourceToBoolConverter : IValueConverter
    {
        public bool Invert { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool returnValue = value is IEnumerable enumerable && enumerable.Cast<object>().Any();
            return !Invert ? returnValue : !returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
            => throw ConverterExceptionMessages.GetNotSupportedConversion(typeof(IEnumerable), typeof(bool));
    }
}
