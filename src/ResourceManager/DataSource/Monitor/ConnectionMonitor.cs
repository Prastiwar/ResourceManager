using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ResourceManager.DataSource
{
    public abstract class ConnectionMonitor : IConnectionMonitor
    {
        public ConnectionMonitor(int interval = 1000) => Interval = interval;

        public event EventHandler<bool> Changed;

        public int Interval { get; }

        public bool IsRunning { get; set; }

        protected int CheckTimeout { get; set; } = 2000;

        protected CancellationTokenSource TokenSource { get; set; }

        protected System.Timers.Timer CheckerTimer { get; set; }

        private bool previousHasConnection;

        public void Stop()
        {
            CheckerTimer?.Stop();
            TokenSource?.Cancel();
            TokenSource = null;
            IsRunning = false;
        }

        public void Start(CancellationToken token = default)
        {
            token.Register(Stop);
            if (CheckerTimer == null)
            {
                CheckerTimer = new System.Timers.Timer(Interval) {
                    AutoReset = false
                };
                CheckerTimer.Elapsed += Elapsed;
            }
            CheckerTimer.Start();
            IsRunning = true;
        }

        private async void Elapsed(object sender, ElapsedEventArgs e)
        {
            using (TokenSource = new CancellationTokenSource(CheckTimeout))
            {
                bool hasConnection = await ForceCheckAsync(TokenSource.Token).ConfigureAwait(true);
                CheckerTimer.Start();
                if (!TokenSource.IsCancellationRequested && hasConnection != previousHasConnection)
                {
                    previousHasConnection = hasConnection;
                    OnChanged(hasConnection);
                }
            }
        }

        public abstract Task<bool> ForceCheckAsync(CancellationToken token);

        protected virtual void OnChanged(bool connection) => Changed?.Invoke(this, connection);
    }
}
