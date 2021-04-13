using System;
using System.Threading;

namespace RPGDataEditor.Core.Services
{
    public interface IConnectionService
    {
        public event EventHandler<bool> ConnectionChanged;

        bool IsChecking { get; }

        void StartChecking(CancellationToken token = default);

        void StopChecking();
    }
}
