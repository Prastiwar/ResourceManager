using FluentFTP;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.Ftp
{
    public class FtpConnectionMonitor : ConnectionMonitor, IDisposable
    {
        public FtpConnectionMonitor(string host, NetworkCredential credentials, int port = 0, int interval = 1000) : base(interval)
        {
            Host = host;
            Client = new FtpClient(host) {
                ConnectTimeout = Timeout,
                DataConnectionConnectTimeout = Timeout,
                DataConnectionReadTimeout = Timeout,
                ReadTimeout = Timeout,
                Port = port,
            };
            if (credentials != null)
            {
                Client.Credentials = credentials;
            }
        }

        public FtpConnectionMonitor(string host, int interval = 1000) : this(host, null, 0, interval) { }

        public string Host { get; }

        protected int Timeout { get; } = 1000;

        protected FtpClient Client { get; }

        private bool disposed;

        public override async Task<bool> ForceCheckAsync(CancellationToken token)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(FtpConnectionMonitor));
            }
            try
            {
                await Client.ConnectAsync(token).ConfigureAwait(true);
                await Client.DisconnectAsync(token).ConfigureAwait(true);
            }
            catch (Exception ex)
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
                    Stop();
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
