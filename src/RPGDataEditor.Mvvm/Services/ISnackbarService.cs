using System;

namespace RPGDataEditor.Mvvm.Services
{
    public interface ISnackbarService
    {
        TimeSpan Duration { get; set; }

        void Clear();

        void Enqueue(object content);

        void Enqueue(object content, object actionContent, Action actionHandler);

        void Enqueue<TArgument>(object content, object actionContent, Action<TArgument> actionHandler, TArgument actionArgument);
    }
}
