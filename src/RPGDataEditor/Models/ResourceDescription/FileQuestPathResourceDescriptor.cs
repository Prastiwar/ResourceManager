using ResourceManager.Data;
using RPGDataEditor.Models;

namespace RPGDataEditor.Services
{
    public class FileQuestPathResourceDescriptor : PathResourceDescriptor
    {
        public FileQuestPathResourceDescriptor() : base(typeof(Quest), "quests", "/{category}/{id}_{name}") { }
    }
}
