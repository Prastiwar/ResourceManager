using System;
using System.Threading;
using System.Threading.Tasks;

namespace RPGDataEditor.Core
{
    public abstract class ConnectionChecker
    {
        public ConnectionChecker(int interval = 1000) => Interval = interval;

        public event EventHandler<bool> Changed;

        public int Interval { get; }

        private bool previousHasConnection;

        public async Task StartAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                bool hasConnection = await ForceCheckAsync();
                if (hasConnection != previousHasConnection)
                {
                    OnChanged(hasConnection);
                    previousHasConnection = hasConnection;
                }
                await Task.Delay(Interval, token);
            }
        }

        public abstract Task<bool> ForceCheckAsync();

        protected virtual void OnChanged(bool connection) => Changed?.Invoke(this, connection);
    }
}
