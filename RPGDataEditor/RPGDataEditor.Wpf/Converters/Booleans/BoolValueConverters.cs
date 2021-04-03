using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Converters
{
    public class BoolValueConverter<T> : IValueConverter where T : IComparable
    {
        public T TrueValue { get; set; }
        public T FalseValue { get; set; }
        public bool Invert { get; set; }

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture) => ConvertoToGeneric(value);

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => ConvertToBool(value);

        protected T ConvertoToGeneric(object value) => !Invert
                                                    ? (value is bool boolean ? boolean ? TrueValue : FalseValue : FalseValue)
                                                    : (value is bool boolean1 ? boolean1 ? FalseValue : TrueValue : TrueValue);

        protected bool ConvertToBool(object value) => !Invert
                                                 ? (value is T val1 && EqualityComparer<T>.Default.Equals(val1, TrueValue))
                                                 : (!(value is T val2) || (!EqualityComparer<T>.Default.Equals(val2, TrueValue)));

    }

    public class BoolValueInvertedConverter<T> : BoolValueConverter<T>, IValueConverter where T : IComparable
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => ConvertToBool(value);
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => ConvertoToGeneric(value);
    }

    public class BoolToScrollVisibilityConverter : BoolValueConverter<ScrollBarVisibility>
    {
        public BoolToScrollVisibilityConverter()
        {
            TrueValue = ScrollBarVisibility.Visible;
            FalseValue = ScrollBarVisibility.Hidden;
        }
    }

    public class BoolToTextWrapConverter : BoolValueConverter<TextWrapping>
    {
        public BoolToTextWrapConverter()
        {
            TrueValue = TextWrapping.Wrap;
            FalseValue = TextWrapping.NoWrap;
        }
    }

    public class BoolToVisibilityConverter : BoolValueConverter<Visibility>
    {
        public BoolToVisibilityConverter()
        {
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Collapsed;
        }
    }

    public class BoolToIntegerConverter : BoolValueConverter<int>
    {
        public BoolToIntegerConverter()
        {
            TrueValue = 1;
            FalseValue = 0;
        }
    }

    public class BoolToInvertBoolConverter : BoolValueConverter<bool>
    {
        public BoolToInvertBoolConverter()
        {
            TrueValue = true;
            FalseValue = false;
            Invert = true;
        }
    }
}
