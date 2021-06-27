using System;
using System.Globalization;
using System.Windows.Data;

namespace ResourceManager.Wpf.Converters
{
    public static class StaticValueConverter
    {
        private class ValueConverter<TConvertTo> : IValueConverter
        {
            private readonly Func<object, TConvertTo> convert;
            private readonly Func<TConvertTo, object> convertBack;

            public ValueConverter(Func<object, TConvertTo> convert, Func<TConvertTo, object> convertBack)
            {
                this.convert = convert ?? throw new ArgumentNullException(nameof(convert));
                this.convertBack = convertBack;
            }

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => convert.Invoke(value);
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => convertBack?.Invoke((TConvertTo)value);
        }

        private class MultiValueConverter<TConvertTo> : IMultiValueConverter
        {
            private readonly Func<object[], TConvertTo> convert;
            private readonly Func<TConvertTo, object[]> convertBack;

            public MultiValueConverter(Func<object[], TConvertTo> convert, Func<TConvertTo, object[]> convertBack)
            {
                this.convert = convert ?? throw new ArgumentNullException(nameof(convert));
                this.convertBack = convertBack;
            }

            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => convert.Invoke(values);
            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => convertBack?.Invoke((TConvertTo)value);
        }

        public static IValueConverter Create<TConvertTo>(Func<object, TConvertTo> convert, Func<TConvertTo, object> convertBack)
            => new ValueConverter<TConvertTo>(convert, convertBack);

        public static IMultiValueConverter CreateMulti<TConvertTo>(Func<object[], TConvertTo> convert, Func<TConvertTo, object[]> convertBack)
            => new MultiValueConverter<TConvertTo>(convert, convertBack);
    }
}
