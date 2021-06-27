using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace ResourceManager.Wpf.Converters
{
    public class CombinePropertyPathConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
            {
                return null;
            }
            StringBuilder builder = new StringBuilder();
            builder.AppendJoin('.', values);
            return builder.ToString();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => value?.ToString().Split('.', StringSplitOptions.RemoveEmptyEntries);
    }
}