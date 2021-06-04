using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ResourceManager.Data
{
    public class Resource : IResource, IEquatable<IResource>, IEquatable<Resource>
    {
        public Resource() { }

        public Resource(Type fromType) => InitializeFromType(fromType);

        public Resource(object fromContext)
        {
            if (fromContext is null)
            {
                throw new ArgumentNullException(nameof(fromContext));
            }
            if (fromContext is IResource resource)
            {
                FromOtherResource(resource);
            }
            else
            {
                InitializeFromType(fromContext.GetType());
                UpdateProperties(fromContext);
            }
        }

        public Resource(IResource fromResource) => FromOtherResource(fromResource);

        private IDictionary<string, ResourceProperty> properties = new Dictionary<string, ResourceProperty>();

        protected void Add<T>(string propertyName, T defaultValue = default) => Add(propertyName, new ResourceProperty(propertyName, typeof(T), defaultValue));

        protected void Add(string propertyName, Type type, object defaultValue = default) => Add(propertyName, new ResourceProperty(propertyName, type, defaultValue));

        protected void Add(string propertyName, ResourceProperty description)
        {
            if (typeof(IResource).IsAssignableFrom(description.Type) && description.Value == null)
            {
                throw new ArgumentException($"Property of type {typeof(IResource)} cannot not be null", nameof(description));
            }
            properties[propertyName] = description;
        }

        public virtual object this[string propertyName] {
            get {
                if (properties.TryGetValue(propertyName, out ResourceProperty property))
                {
                    return property.Value;
                }
                else
                {
                    throw new ArgumentException($"Property with name {propertyName} has not been found", nameof(propertyName));
                }
            }
            set {
                if (properties.TryGetValue(propertyName, out ResourceProperty property))
                {
                    property.Value = value;
                }
                else
                {
                    throw new ArgumentException($"Property with name {propertyName} has not been found", nameof(propertyName));
                }
            }
        }

        public T GetValue<T>(string propertyName) => CanAcceptPropertyType(typeof(T))
                ? (T)this[propertyName]
                : throw new InvalidCastException($"{typeof(T)} is not object (or collection) of primitive or resource");

        protected void FromOtherResource(IResource resource) => properties = new Dictionary<string, ResourceProperty>(resource.GetProperties());

        /// <summary> Returns true if type (or collection's element type) is primitive, string or IResource </summary>
        protected bool CanAcceptPropertyType(Type type)
        {
            bool canAccept = type.IsPrimitive || type == typeof(string) || typeof(IResource).IsAssignableFrom(type);
            if (!canAccept)
            {
                if (type.IsEnumerable())
                {
                    return CanAcceptPropertyType(type.GetEnumerableElementType());
                }
            }
            return canAccept;
        }

        protected void InitializeFromType(Type type = null)
        {
            if (type == null)
            {
                type = GetType();
            }
            if (type.IsEnumerable())
            {
                throw new InvalidOperationException("Collection cannot be created as Resource");
            }
            foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(prop => prop.GetGetMethod() != null && prop.GetSetMethod() != null))
            {
                Type declaredType = property.PropertyType;
                Type propertyType = null;
                object value = null;
                if (CanAcceptPropertyType(declaredType))
                {
                    if (!declaredType.IsEnumerable())
                    {
                        value = Activator.CreateInstance(declaredType);
                    }
                }
                else
                {
                    if (declaredType.IsArray)
                    {
                        value = Array.Empty<IResource>();
                    }
                    else if (declaredType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(declaredType))
                    {
                        try
                        {
                            Type enumerableType = declaredType.GetGenericTypeDefinition().MakeGenericType(typeof(IResource));
                            value = Activator.CreateInstance(enumerableType);
                        }
                        catch (Exception)
                        {
                            value = new List<IResource>();
                        }
                    }
                    else
                    {
                        value = ConvertToResource(declaredType);
                    }
                }
                if (value != null)
                {
                    propertyType = value.GetType();
                }
                Add(property.Name, new ResourceProperty(property.Name, propertyType ?? declaredType, declaredType, value));
            }
        }

        public void UpdateProperties(object context)
        {
            foreach (KeyValuePair<string, ResourceProperty> property in GetProperties())
            {
                string propertyName = property.Key;
                PropertyInfo propertyInfo = context.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo != null && propertyInfo.GetGetMethod() != null)
                {
                    Type propertyType = propertyInfo.PropertyType;
                    object propertyValue = propertyInfo.GetValue(context);
                    if (CanAcceptPropertyType(propertyType))
                    {
                        if (!propertyType.IsEnumerable())
                        {
                            property.Value.Value = propertyInfo.GetValue(context);
                        }
                        else
                        {
                            //property.Value.Value = ConvertToCollection(propertyValue, propertyInfo.PropertyType);
                        }
                    }
                    else
                    {
                        if (propertyValue is null)
                        {
                            property.Value.Value = null;
                        }
                        else if (propertyValue is IEnumerable<object> enumerable)
                        {
                            List<IResource> resources = new List<IResource>();
                            foreach (object element in enumerable)
                            {
                                Resource resourceElement = ConvertToResource(element.GetType(), element);
                                resources.Add(resourceElement);
                            }
                            if (property.Value.Type.IsArray)
                            {
                                property.Value.Value = resources.ToArray();
                            }
                            else
                            {
                                //property.Value.Value = ConvertToCollection(property.Value.Type, resources);
                            }
                        }
                        else
                        {
                            Resource resource = ConvertToResource(propertyInfo.PropertyType, propertyValue);
                            property.Value.Value = resource;
                        }
                    }
                }
            }
        }

        //protected virtual object ConvertToCollection(Type propertyType, List<IResource> resources)
        //{
        //    try
        //    {
        //        return Activator.CreateInstance(propertyType, resources);
        //    }
        //    catch (Exception)
        //    {
        //        return resources;
        //    }
        //}

        protected virtual Resource ConvertToResource(Type type, object context = null)
        {
            Resource resource = new Resource(type);
            if (context != null)
            {
                resource.UpdateProperties(context);
            }
            return resource;
        }

        public TModel ToModel<TModel>(TModel model = default)
        {
            if (model is null)
            {
                model = Activator.CreateInstance<TModel>();
            }
            foreach (KeyValuePair<string, ResourceProperty> property in GetProperties())
            {
                string propertyName = property.Key;
                PropertyInfo propertyInfo = model.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                // if (propertyInfo != null && propertyInfo.GetSetMethod() != null && propertyInfo.PropertyType.IsAssignableFrom(property.Value.Type))
                if (propertyInfo != null)
                {
                    // propertyInfo.SetValue(model, property.Value.Value);;
                }
            }
            return model;
        }

        public IEnumerable<KeyValuePair<string, ResourceProperty>> GetProperties() => properties;

        public ResourceProperty GetProperty(string propertyName) => properties[propertyName];

        public override int GetHashCode() => HashCode.Combine(properties);

        public override bool Equals(object obj) => Equals(obj as Resource);

        public bool Equals(Resource other) => other != null && Equals(other as IResource);

        public bool Equals(IResource other) => other != null &&
                                               other.GetProperties() is IDictionary<string, ResourceProperty> dictionary ? properties.EquivalentTo(dictionary)
                                                                                                                            : GetProperties().OrderBy(x => x.Key)
                                                                                                                                             .SequenceEqual(other.GetProperties());

        public static bool operator ==(Resource left, Resource right) => EqualityComparer<Resource>.Default.Equals(left, right);

        public static bool operator !=(Resource left, Resource right) => !(left == right);
    }
}
