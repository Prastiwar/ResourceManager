using ResourceManager.Data;
using RPGDataEditor.Models;

namespace RPGDataEditor.Services
{
    public class SqlQuestResourceDescriptor : NameResourceDescriptor
    {
        public SqlQuestResourceDescriptor() : base(typeof(QuestModel), "quests") { }
    }
}
