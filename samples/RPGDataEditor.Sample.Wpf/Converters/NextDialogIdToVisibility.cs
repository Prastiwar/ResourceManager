using RPGDataEditor.Wpf.Converters;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RPGDataEditor.Sample.Wpf.Converters
{
    /// <summary> Converts integer id to Visibility. Returns Visibility.Visible if id is greater or equal to 0 </summary>
    public class NextDialogIdToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is int id ? id >= 0 ? Visibility.Visible : Visibility.Collapsed: Visibility.Collapsed;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
            => throw ConverterExceptionMessages.GetNotSupportedConversion(typeof(Visibility), typeof(int));
    }
}
