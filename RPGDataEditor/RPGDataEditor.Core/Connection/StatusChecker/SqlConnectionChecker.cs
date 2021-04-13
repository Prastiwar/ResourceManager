using System;
using System.Threading;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Connection
{
    public class SqlConnectionChecker : ConnectionChecker, IDisposable
    {
        public SqlConnectionChecker(string connectionString, int interval = 1000) : base(interval)
        {
            ConnectionString = connectionString;
            // Connection = new SqlConnection(ConnectionString);
        }

        public string ConnectionString { get; }

        // protected SqlConnection Connection { get; }

        private bool disposed;

        public override Task<bool> ForceCheckAsync(CancellationToken token)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(SqlConnectionChecker));
            }
            return Task.FromResult(false);
            // try
            // {
            //     connection.Open();
            //     connection.Close();
            //     return true;
            // }
            // catch (SqlException)
            // {
            //     return false;
            // }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Connection.Close();
                    // Connection.Dispose();
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
