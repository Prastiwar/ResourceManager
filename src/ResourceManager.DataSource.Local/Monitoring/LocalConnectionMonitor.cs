using System;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.Local
{
    public class LocalConnectionMonitor : IConnectionMonitor
    {
        public bool IsRunning => false;

        public event EventHandler<bool> Changed;

        public Task<bool> ForceCheckAsync(CancellationToken token) => Task.FromResult(true);

        public void Start(CancellationToken token = default) { }

        public void Stop() { }
    }
}
