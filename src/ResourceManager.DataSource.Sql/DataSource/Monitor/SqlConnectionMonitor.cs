using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.Sql
{
    public class SqlConnectionMonitor : ConnectionMonitor
    {
        public SqlConnectionMonitor(DbContext context, int interval = 1000)
            : base(interval) => Context = context;

        public DbContext Context { get; }

        public override async Task<bool> ForceCheckAsync(CancellationToken token) => await Context.Database.CanConnectAsync();
    }
}
