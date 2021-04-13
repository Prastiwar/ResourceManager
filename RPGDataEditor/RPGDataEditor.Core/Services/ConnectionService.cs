using RPGDataEditor.Core.Connection;
using RPGDataEditor.Core.Mvvm;
using RPGDataEditor.Core.Providers;
using System;
using System.Threading;

namespace RPGDataEditor.Core.Services
{
    public class ConnectionService : IConnectionService
    {
        public ConnectionService(ISessionContext session, IConnectionCheckerProvider checkerProvider)
        {
            Session = session;
            CheckerProvider = checkerProvider;
        }

        protected ISessionContext Session { get; }
        protected IConnectionCheckerProvider CheckerProvider { get; }

        protected IConnectionChecker Checker { get; set; }

        public event EventHandler<bool> ConnectionChanged;

        public bool IsChecking => Checker != null && Checker.IsRunning;

        public void StartChecking(CancellationToken token = default)
        {
            if (Checker != null)
            {
                Checker.Stop();
            }
            Checker = CheckerProvider.GetConnectionChecker(Session.Client);
            Checker.Changed += OnConnectionChanged;
            Checker.Start(token);
        }

        protected virtual void OnConnectionChanged(object sender, bool e) => ConnectionChanged?.Invoke(this, e);

        public void StopChecking()
        {
            Checker?.Stop();
            Checker = null;
        }
    }
}
