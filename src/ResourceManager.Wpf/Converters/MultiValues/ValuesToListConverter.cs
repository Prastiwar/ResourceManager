using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace ResourceManager.Wpf.Converters
{
    public class ValuesToListConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => new List<object>(values);

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => value is IList list ? list.Cast<object>().ToArray() : null;
    }
}