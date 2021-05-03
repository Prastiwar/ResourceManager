using ResourceManager.Data;
using RPGDataEditor.Models;

namespace RPGDataEditor.Services
{
    public class SqlDialoguePathResourceDescriptor : PathResourceDescriptor
    {
        public SqlDialoguePathResourceDescriptor() : base(typeof(Quest), "dialogues", ".{id}") { }
    }
}
