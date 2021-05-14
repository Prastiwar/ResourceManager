using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace RPGDataEditor.Wpf.Converters
{
    public abstract class ProxyConnectionSettings : IProxyConnectionSettings, IEquatable<IConfiguration>
    {
        public ProxyConnectionSettings(IConfiguration settings) => Settings = settings;

        public IConfiguration Settings { get; }

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

        public bool Equals([AllowNull] IConfiguration other) => other == Settings;
    }
}