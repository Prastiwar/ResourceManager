using RPGDataEditor.Connection;
using System;
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
            get {
                object value = Get();
                if (value is string password)
                {
                    SecureString secureValue = new SecureString();
                    foreach (char character in password)
                    {
                        secureValue.AppendChar(character);
                    }
                    value = secureValue;
                    Set(value);
                }
                return (SecureString)Get();
            }

            set => Set(value);
        }

        public int Port {
            get => Convert.ToInt32(Get());
            set => Set(value);
        }
    }
}
