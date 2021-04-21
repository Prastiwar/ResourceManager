using System;
using System.Threading;

namespace RPGDataEditor.Services
{
    public interface ICheckConnectionService
    {
        public event EventHandler<bool> ConnectionChanged;

        bool IsChecking { get; }

        void StartChecking(CancellationToken token = default);

        void StopChecking();
    }
}
