using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Converters
{
    public class ItemsSourceToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable enumrable)
            {
                return enumrable.Cast<object>().Any();
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => new NotSupportedException("Reverting bool to ienumerable is not supported");
    }
}
