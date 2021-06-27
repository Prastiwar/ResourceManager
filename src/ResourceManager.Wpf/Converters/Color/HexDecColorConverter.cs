using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ResourceManager.Wpf.Converters
{
    public class HexDecColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string hexValue = ((int)value).ToString("X");
            if (hexValue.Length != 6)
            {
                int lengthMissing = 6 - hexValue.Length;
                string zeros = "";
                for (int i = 0; i < lengthMissing; i++)
                {
                    zeros += "0";
                }
                hexValue = zeros + hexValue;
            }
            try
            {
                return ColorConverter.ConvertFromString("#" + hexValue);
            }
            catch (Exception)
            {
                return Colors.Black;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string hexValue = value.ToString();
            if (hexValue.StartsWith("#"))
            {
                hexValue = hexValue.Substring(1);
            }
            try
            {
                return int.Parse(hexValue, NumberStyles.HexNumber);
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
