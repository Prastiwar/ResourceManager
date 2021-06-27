using System;
using System.Globalization;

namespace ResourceManager.Wpf.Converters
{
    public class ParameterProxyConnectionSettingsConverter : ProxyConnectionSettingsConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TargetProxyType = parameter as Type;
            return base.Convert(value, targetType, parameter, culture);
        }
    }
}
