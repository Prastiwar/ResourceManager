using ResourceManager.Data;
using System;
using System.Globalization;
using System.Windows.Data;

namespace ResourceManager.Wpf.Converters
{
    public class IdentifiableToIdConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is IIdentifiable identifiable ? identifiable.Id : -1;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => ConverterExceptionMessages.GetNotSupportedConversion(typeof(int), typeof(IIdentifiable));
    }
}
