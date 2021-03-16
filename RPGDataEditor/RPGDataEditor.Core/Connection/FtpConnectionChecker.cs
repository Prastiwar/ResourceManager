using FluentFTP;
using System;
using System.Net;
using System.Threading.Tasks;

namespace RPGDataEditor.Core
{
    public class FtpConnectionChecker : ConnectionChecker, IDisposable
    {
        public FtpConnectionChecker(string host, NetworkCredential credentials, int port, int interval = 1000) : base(interval)
        {
            Host = host;
            Client = new FtpClient(host) {
                Port = port,
            };
            if (credentials != null)
            {
                Client.Credentials = credentials;
            }
        }

        public FtpConnectionChecker(string host, int interval = 1000) : this(host, null, 0, interval) { }

        public string Host { get; }

        protected FtpClient Client { get; }

        private bool disposed;

        public override async Task<bool> ForceCheckAsync()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(FtpConnectionChecker));
            }
            try
            {
                await Client.ConnectAsync();
                await Client.DisconnectAsync();
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Client.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
