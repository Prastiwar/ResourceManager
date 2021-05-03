using Prism.Ioc;
using RPGDataEditor.Connection;
using System;

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
            // TODO: resolve checker based on config
            //Checker = 
            OnConfigured(config);
        }

        protected void OnConfigured(IConnectionConfig config) => Configured?.Invoke(this, config);

        public IConnectionConfig Config { get; protected set; }

        public IConnectionChecker Checker { get; protected set; }

    }
}
