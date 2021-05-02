using RPGDataEditor.Connection;
using System.Runtime.CompilerServices;

namespace RPGDataEditor.Wpf.Converters
{
    public abstract class ProxyConnectionSettings : IProxyConnectionSettings
    {
        public ProxyConnectionSettings(IConnectionSettings settings) => Settings = settings;

        public IConnectionSettings Settings { get; }

        protected object Get([CallerMemberName] string parameter = null) => Settings.Get(parameter);

        protected void Set(object value, [CallerMemberName] string parameter = null) => Settings.Set(parameter, value);
    }
}