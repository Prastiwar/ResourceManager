using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.Sql
{
    public class SqlConnectionMonitor : ConnectionMonitor, IDisposable
    {
        public SqlConnectionMonitor(string connectionString, int interval = 1000) : base(interval)
        {
            ConnectionString = connectionString;
            Connection = new SqlConnection(ConnectionString);
        }

        public string ConnectionString { get; }

        protected SqlConnection Connection { get; }

        private bool disposed;

        public override async Task<bool> ForceCheckAsync(CancellationToken token)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(SqlConnectionMonitor));
            }
            try
            {
                await Connection.OpenAsync();
                await Connection.CloseAsync();
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Connection.Close();
                    Connection.Dispose();
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
