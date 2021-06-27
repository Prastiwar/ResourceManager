using System;

namespace Extensions.Configuration.WriteableJson.Tests
{
    public class DisposableTrigger : IDisposable
    {
        public DisposableTrigger(Action trigger) => this.trigger = trigger;

        private readonly Action trigger;

        public void Dispose() => trigger.Invoke();
    }
}
