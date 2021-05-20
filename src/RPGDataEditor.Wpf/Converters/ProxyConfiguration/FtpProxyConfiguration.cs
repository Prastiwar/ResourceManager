using Microsoft.Extensions.Configuration;
using ResourceManager;
using System;
using System.Security;

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

        public SecureString Password {
            get => SecretString.DecryptString(Get());
            set => Set(SecretString.EncryptString(value));
        }

        public int Port {
            get => Convert.ToInt32(Get());
            set => Set(value);
        }
    }
}
