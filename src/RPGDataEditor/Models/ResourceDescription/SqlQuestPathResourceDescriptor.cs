using ResourceManager.Data;
using RPGDataEditor.Models;

namespace RPGDataEditor.Services
{
    public class SqlQuestPathResourceDescriptor : PathResourceDescriptor
    {
        public SqlQuestPathResourceDescriptor() : base(typeof(Quest), "quests", ".{id}") { }
    }
}
