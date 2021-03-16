using System;
using System.Globalization;
using System.Windows.Data;
using System.Text;

namespace RPGDataEditor.Wpf.Converters
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

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotSupportedException("Converting combined property paths back is not supported");
    }
}