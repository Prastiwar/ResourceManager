using System.ComponentModel;

namespace ResourceManager.Data
{
    public interface IObservableResource : IResource, INotifyPropertyChanged { }
}
