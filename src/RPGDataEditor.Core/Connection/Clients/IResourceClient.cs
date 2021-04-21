using ResourceManager.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Connection
{
    public interface IResourceClient
    {
        Task<IEnumerable<object>> ListResources(IResourceDescriptor descriptor);
    }
}
