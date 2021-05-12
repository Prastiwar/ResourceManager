using RPGDataEditor.Connection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace RPGDataEditor.Wpf.Converters
{
    public abstract class ProxyConnectionSettings : IProxyConnectionSettings, IEquatable<IConnectionSettings>
    {
        public ProxyConnectionSettings(IConnectionSettings settings) => Settings = settings;

        public IConnectionSettings Settings { get; }

        protected object Get([CallerMemberName] string parameter = null)
        {
            try
            {
                return Settings.Get(parameter);
            }
            catch (Exception)
            {
                Settings.Set(parameter, null);
                return null;
            }
        }

        protected void Set(object value, [CallerMemberName] string parameter = null) => Settings.Set(parameter, value);

        public bool Equals([AllowNull] IConnectionSettings other) => other == Settings;
    }
}