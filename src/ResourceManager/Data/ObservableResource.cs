using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ResourceManager.Data
{
    public class ObservableResource : Resource, IObservableResource
    {
        public ObservableResource() { }

        public ObservableResource(Type fromType)
        {
            InitializeFromType(fromType);
            EnsureObservableResources();
        }

        public ObservableResource(object fromContext)
        {
            if (fromContext is null)
            {
                throw new ArgumentNullException(nameof(fromContext));
            }
            if (fromContext is IResource resource)
            {
                FromOtherResource(resource);
                EnsureObservableResources();
            }
            else
            {
                InitializeFromType(fromContext.GetType());
                UpdateProperties(fromContext);
            }
        }

        public ObservableResource(IResource fromResource)
        {
            FromOtherResource(fromResource);
            EnsureObservableResources();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override object this[string propertyName] {
            get => base[propertyName];
            set {
                if (value is IResource resource && !(value is IObservableResource))
                {
                    value = new ObservableResource(resource); // ensure property resource is observable
                }
                base[propertyName] = value;
                OnPropertyChanged(propertyName);
                OnPropertyChanged("Item[]");
            }
        }

        protected override Resource ConvertToResource(Type type, object context = null)
        {
            ObservableResource resource = new ObservableResource(type);
            if (context != null)
            {
                resource.UpdateProperties(context);
            }
            return resource;
        }

        // Replaces properties of type IResource with ObservableResource
        protected void EnsureObservableResources()
        {
            foreach (KeyValuePair<string, ResourceProperty> property in GetProperties())
            {
                if (typeof(IResource).IsAssignableFrom(property.Value.Type) && !(property.Value.Value is IObservableResource))
                {
                    property.Value.Value = new ObservableResource(property.Value.Value as IResource);
                }
                else if (!property.Value.Type.IsArray && property.Value.Type.IsEnumerable() && !(typeof(INotifyCollectionChanged).IsAssignableFrom(property.Value.Type)))
                {
                    Type elementType = property.Value.Type.GetEnumerableElementType();
                    if (typeof(IResource).IsAssignableFrom(elementType))
                    {
                        if (property.Value.Value == null)
                        {
                            property.Value.Value = new ObservableCollection<IResource>();
                        }
                        else
                        {
                            property.Value.Value = new ObservableCollection<IResource>((IEnumerable<IResource>)property.Value.Value);
                        }
                    }
                    else
                    {
                        Type collectionType = typeof(ObservableCollection<>).MakeGenericType(elementType);
                        if (property.Value.Value == null)
                        {
                            property.Value.Value = Activator.CreateInstance(collectionType);
                        }
                        else
                        {
                            property.Value.Value = Activator.CreateInstance(collectionType, (IEnumerable<IResource>)property.Value.Value);
                        }
                    }
                }
            }
        }

        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
