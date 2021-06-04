using System.Collections.Generic;

namespace ResourceManager.Data
{
    public interface IResource
    {
        object this[string propertyName] { get; set; }

        T GetValue<T>(string propertyName);

        ResourceProperty GetProperty(string propertyName);

        IEnumerable<KeyValuePair<string, ResourceProperty>> GetProperties();
    }
}
