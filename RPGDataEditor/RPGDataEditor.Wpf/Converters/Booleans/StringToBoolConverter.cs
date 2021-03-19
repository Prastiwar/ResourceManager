using System;
using System.Globalization;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Converters
{
    public class StringToBoolConverter : IValueConverter
    {
        public bool Invert { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool equal = value.ToString() == parameter.ToString();
            return Invert ? !equal : equal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
            => throw new NotSupportedException("Convert bool to proper string value is not suppoerted");
    }
}
