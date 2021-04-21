using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Converters
{
    public abstract class SimpleInvertableConverter<T> : IValueConverter
    {
        public T PositiveValue { get; set; }
        public T NegativeValue { get; set; }

        public bool Invert { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            T returnValue = ConvertTo(value, targetType, parameter, culture);
            if (!Invert)
            {
                return returnValue;
            }
            return EqualityComparer<T>.Default.Equals(returnValue, PositiveValue) ? NegativeValue : PositiveValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            T actualValue = (T)value;
            if (Invert)
            {
                actualValue = EqualityComparer<T>.Default.Equals(actualValue, PositiveValue) ? NegativeValue : PositiveValue;
            }
            return ConvertToBack(actualValue, targetType, parameter, culture);
        }

        public abstract T ConvertTo(object value, Type targetType, object parameter, CultureInfo culture);
        public abstract object ConvertToBack(T value, Type targetType, object parameter, CultureInfo culture);
    }
}
