using RPGDataEditor.Core;
using System;
using System.Globalization;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Converters
{
    public class IdentifiableToIdConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
            => value is IIdentifiable identifiable ? identifiable.Id : -1;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
            => throw new NotSupportedException("Cannot return Identifiable from id");
    }
}
