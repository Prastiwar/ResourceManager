using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using System.Windows.Data;

namespace ResourceManager.Wpf.Converters
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
            if (!typeof(IProxyConfiguration).IsAssignableFrom(TargetProxyType))
            {
                throw new InvalidOperationException($"{nameof(TargetProxyType)} is invalid type. It should be assignable to {typeof(IProxyConfiguration)}");
            }
            if (value is IConfiguration configuration)
            {
                return Activator.CreateInstance(TargetProxyType, new object[] { configuration });
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value is IProxyConfiguration proxy ? System.Convert.ChangeType(proxy.Configuration, targetType) : null;
    }
}
