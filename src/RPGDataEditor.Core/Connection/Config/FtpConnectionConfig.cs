using System.Security;

namespace RPGDataEditor.Core.Connection
{
    public class FtpConnectionConfig : ConnectionConfig
    {
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
