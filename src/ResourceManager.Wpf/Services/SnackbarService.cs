using MaterialDesignThemes.Wpf;
using ResourceManager.Mvvm.Services;
using System;

namespace ResourceManager.Wpf.Services
{
    public class SnackbarService : ISnackbarService
    {
        public SnackbarService()
        {
            MessageQueue = new SnackbarMessageQueue();
            Duration = TimeSpan.FromSeconds(3);
        }

        public SnackbarMessageQueue MessageQueue { get; }

        public TimeSpan Duration { get; set; }

        public void Clear() => MessageQueue.Clear();

        public void Enqueue(object content) => MessageQueue.Enqueue(content, null, null, null, false, false, Duration);

        public void Enqueue(object content, object actionContent, Action actionHandler) => MessageQueue.Enqueue(content, actionContent, (obj) => actionHandler?.Invoke(), null, false, false, Duration);

        public void Enqueue<TArgument>(object content, object actionContent, Action<TArgument> actionHandler, TArgument actionArgument)
            => MessageQueue.Enqueue(content, actionContent, actionHandler, actionArgument, false, false, Duration);

    }
}
