using ResourceManager.Data;
using RPGDataEditor.Models;

namespace RPGDataEditor.Services
{
    public class FileNpcPathResourceDescriptor : PathResourceDescriptor
    {
        public FileNpcPathResourceDescriptor() : base(typeof(Quest), "npcs", "/{id}_{name}") { }
    }
}
