using RPGDataEditor.Connection;
using System.Security;

namespace RPGDataEditor.Wpf.Converters
{
    public class FtpProxyConnectionSettings : ProxyConnectionSettings
    {
        public FtpProxyConnectionSettings(IConnectionSettings settings) : base(settings) { }

        public string Host {
            get => (string)Get();
            set => Set(value);
        }

        public string RelativePath {
            get => (string)Get();
            set => Set(value);
        }

        public string UserName {
            get => (string)Get();
            set => Set(value);
        }

        public SecureString Password {
            get => (SecureString)Get();
            set => Set(value);
        }

        public int Port {
            get => (int)Get();
            set => Set(value);
        }
    }
}
