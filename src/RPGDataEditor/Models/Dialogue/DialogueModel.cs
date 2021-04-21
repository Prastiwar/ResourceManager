using System.Collections.Generic;

namespace RPGDataEditor.Models
{
    public class DialogueModel : IdentifiableData
    {
        public object StartQuestId { get; set; }

        public IList<DialogueOptionModel> Options { get; private set; }
    }
}
