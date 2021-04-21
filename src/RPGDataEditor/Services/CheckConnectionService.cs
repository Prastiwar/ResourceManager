using RPGDataEditor.Connection;
using System;
using System.Threading;

namespace RPGDataEditor.Services
{
    public abstract class CheckConnectionService : ICheckConnectionService
    {
        protected IConnectionChecker Checker { get; set; }

        public event EventHandler<bool> ConnectionChanged;

        public bool IsChecking => Checker != null && Checker.IsRunning;

        public void StartChecking(CancellationToken token = default)
        {
            if (Checker != null)
            {
                Checker.Stop();
            }
            Checker = CreateChecker();
            Checker.Changed += OnConnectionChanged;
            Checker.Start(token);
        }

        protected virtual void OnConnectionChanged(object sender, bool e) => ConnectionChanged?.Invoke(this, e);

        public void StopChecking()
        {
            Checker?.Stop();
            Checker = null;
        }

        protected abstract IConnectionChecker CreateChecker();
    }
}
