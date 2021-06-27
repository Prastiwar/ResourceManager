using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ResourceManager.Wpf.Converters
{
    public class ColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => new SolidColorBrush((Color)value);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value is SolidColorBrush brush ? brush.Color : new Color();
    }
}
