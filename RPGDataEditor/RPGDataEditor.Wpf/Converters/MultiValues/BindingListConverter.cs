using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Linq;

namespace RPGDataEditor.Wpf.Converters
{
    public class BindingListConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => new List<object>(values);

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if(value is IList list) {
                list.Cast<object>().ToArray();
            }
            return null; 
        }
    }
}