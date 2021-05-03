using ResourceManager.Data;
using RPGDataEditor.Models;

namespace RPGDataEditor.Services
{
    public class FileDialoguePathResourceDescriptor : PathResourceDescriptor
    {
        public FileDialoguePathResourceDescriptor() : base(typeof(Quest), "dialogues", "/{category}/{id}_{name}") { }
    }
}
