using Microsoft.Extensions.Configuration;
using System;

namespace ResourceManager.DataSource
{
    public class ConfigurationChangedEventArgs : EventArgs
    {
        public ConfigurationChangedEventArgs(IDataSource oldValue, IConfiguration configuration)
        {
            OldValue = oldValue;
            Configuration = configuration;
        }

        public IDataSource OldValue { get; }
        public IConfiguration Configuration { get; }
    }
}
