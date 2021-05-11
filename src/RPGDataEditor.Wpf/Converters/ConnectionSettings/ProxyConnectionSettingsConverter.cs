using RPGDataEditor.Connection;
using System;
using System.Globalization;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Converters
{
    public class ProxyConnectionSettingsConverter : IValueConverter
    {
        public Type TargetProxyType { get; set; }

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (TargetProxyType == null)
            {
                throw new ArgumentNullException($"{nameof(TargetProxyType)} is required to create proper proxy instance");
            }
            if (!typeof(IProxyConnectionSettings).IsAssignableFrom(TargetProxyType))
            {
                throw new InvalidOperationException($"{nameof(TargetProxyType)} is invalid type. It should be assignable to {typeof(IProxyConnectionSettings)}");
            }
            if (value is IConnectionSettings settings)
            {
                return Activator.CreateInstance(TargetProxyType, new object[] { settings });
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value is IProxyConnectionSettings proxy ? System.Convert.ChangeType(proxy.Settings, targetType) : null;
    }
}
