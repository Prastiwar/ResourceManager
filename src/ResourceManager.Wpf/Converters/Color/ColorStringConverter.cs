using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace ResourceManager.Wpf.Converters
{
    public class ColorStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = value is Color c ? c : new Color();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("#{0:X2}", color.R);
            sb.AppendFormat("{0:X2}", color.G);
            sb.AppendFormat("{0:X2}", color.B);
            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return ColorConverter.ConvertFromString(value as string);
            }
            catch (Exception)
            {
                return new Color();
            }
        }
    }
}
