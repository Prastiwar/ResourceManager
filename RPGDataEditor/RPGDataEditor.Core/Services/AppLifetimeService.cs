using System;

namespace RPGDataEditor.Core.Services
{
    public class AppLifetimeService : IAppLifetimeService
    {
        public event EventHandler ConnectionEstablished;

        public event EventHandler ConnectionLost;

        public void OnConnectionEstablished() => ConnectionEstablished.Invoke(null, EventArgs.Empty);

        public void OnConnectionLost() => ConnectionLost.Invoke(null, EventArgs.Empty);
    }
}
