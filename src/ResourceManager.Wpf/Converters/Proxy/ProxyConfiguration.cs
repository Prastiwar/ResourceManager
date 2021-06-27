using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ResourceManager.Wpf.Converters
{
    public abstract class ProxyConfiguration : IProxyConfiguration, IEquatable<IConfiguration>
    {
        public ProxyConfiguration(IConfiguration settings) => Configuration = settings;

        public IConfiguration Configuration { get; }

        protected string Get([CallerMemberName] string parameter = null) => Configuration[parameter];

        protected void Set(object value, [CallerMemberName] string parameter = null) => Configuration[parameter] = value.ToString();

        public bool Equals([AllowNull] IConfiguration other) => other == Configuration;
    }
}