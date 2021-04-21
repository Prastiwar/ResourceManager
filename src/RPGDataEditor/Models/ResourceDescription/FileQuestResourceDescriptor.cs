using ResourceManager.Data;
using RPGDataEditor.Models;

namespace RPGDataEditor.Services
{
    public class FileQuestResourceDescriptor : PathResourceDescriptor
    {
        public FileQuestResourceDescriptor() : base(typeof(QuestModel), "quests", "{category}/{name}_{id}") { }
    }
}
