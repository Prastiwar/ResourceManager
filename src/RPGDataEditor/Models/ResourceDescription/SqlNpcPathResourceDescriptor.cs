using ResourceManager.Data;
using RPGDataEditor.Models;

namespace RPGDataEditor.Services
{
    public class SqlNpcPathResourceDescriptor : PathResourceDescriptor
    {
        public SqlNpcPathResourceDescriptor() : base(typeof(Quest), "npcs", ".{id}") { }
    }
}
