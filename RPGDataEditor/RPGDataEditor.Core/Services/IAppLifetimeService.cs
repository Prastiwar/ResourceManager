using System;

namespace RPGDataEditor.Core.Services
{
    public interface IAppLifetimeService
    {
        event EventHandler ConnectionEstablished;
        event EventHandler ConnectionLost;

        void OnConnectionEstablished();
        void OnConnectionLost();
    }
}
