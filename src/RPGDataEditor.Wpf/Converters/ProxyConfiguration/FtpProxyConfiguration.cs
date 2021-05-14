using Microsoft.Extensions.Configuration;
using System;

namespace RPGDataEditor.Wpf.Converters
{
    public class FtpProxyConfiguration : ProxyConfiguration
    {
        public FtpProxyConfiguration(IConfiguration settings) : base(settings) { }

        public string Host {
            get => Get();
            set => Set(value);
        }

        public string RelativePath {
            get => Get();
            set => Set(value);
        }

        public string UserName {
            get => Get();
            set => Set(value);
        }

        public string Password {
            get => Get();
            set => Set(value);
        }

        public int Port {
            get => Convert.ToInt32(Get());
            set => Set(value);
        }
    }
}
