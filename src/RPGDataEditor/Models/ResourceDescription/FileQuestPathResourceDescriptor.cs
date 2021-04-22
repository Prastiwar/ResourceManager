using ResourceManager.Data;
using RPGDataEditor.Models;

namespace RPGDataEditor.Services
{
    public class FileQuestPathResourceDescriptor : PathResourceDescriptor
    {
        public FileQuestPathResourceDescriptor() : base(typeof(QuestModel), "quests", "/{category}/{name}_{id}") { }
    }
}
