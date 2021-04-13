using System;
using System.Threading;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Connection
{
    public interface IConnectionChecker
    {
        public event EventHandler<bool> Changed;

        bool IsRunning { get; }

        void Stop();

        void Start(CancellationToken token = default);

        Task<bool> ForceCheckAsync(CancellationToken token);
    }
}
