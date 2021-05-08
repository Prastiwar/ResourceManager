using MediatR;

namespace ResourceManager.Notifications
{
    public class ResourceChangedNotification<T> : INotification
    {
        public ResourceChangedNotification(T resource, ResourceChangedAction action)
        {
            Resource = resource;
            Action = action;
        }

        public T Resource { get; }
        public ResourceChangedAction Action { get; }
    }
}
