﻿using Microsoft.Extensions.Configuration;

namespace RPGDataEditor.Wpf.Converters
{
    public class SqlProxyConfiguration : ProxyConfiguration
    {
        public SqlProxyConfiguration(IConfiguration settings) : base(settings) { }

        public string ConnectionString {
            get => Get();
            set => Set(value);
        }
    }
}
