using System;
using System.Globalization;
using System.Windows;

namespace ResourceManager.Wpf.Converters
{
    public class VisibilityInvertConverter : SimpleInvertableConverter<Visibility>
    {
        public VisibilityInvertConverter()
        {
            PositiveValue = Visibility.Visible;
            NegativeValue = Visibility.Collapsed;
            Invert = true;
        }

        public override Visibility ConvertTo(object value, Type targetType, object parameter, CultureInfo culture)
            => value is Visibility visibility ? visibility : NegativeValue;

        public override object ConvertToBack(Visibility value, Type targetType, object parameter, CultureInfo culture) => value;
    }
}
