using Prism.Ioc;
using RPGDataEditor.Connection;
using RPGDataEditor.Core.Connection;
using System;
using System.Security;

namespace RPGDataEditor.Wpf.Connection
{
    public class ConnectionConfiguration : IConnectionConfiguration
    {
        public ConnectionConfiguration(IContainerExtension container) => Container = container;

        protected IContainerExtension Container { get; }

        public event EventHandler<IConnectionConfig> Configured;

        public virtual void Configure(IConnectionConfig config)
        {
            Config = config;
            Checker = Config.Get(nameof(ConnectionSettings.Type)).ToString() switch {
                ConnectionSettings.Connection.FTP => new FtpConnectionChecker(Config.Get("Host").ToString(), new System.Net.NetworkCredential(Config.Get("UserName").ToString(), Config.Get("Password") as SecureString)),
                ConnectionSettings.Connection.LOCAL => new LocalConnectionChecker(),
                ConnectionSettings.Connection.SQL => new SqlConnectionChecker(Config.Get("ConnectionString").ToString()),
                _ => null,
            };
            OnConfigured(config);
        }

        protected void OnConfigured(IConnectionConfig config) => Configured?.Invoke(this, config);

        public IConnectionConfig Config { get; protected set; }

        public IConnectionChecker Checker { get; protected set; }

    }
}
