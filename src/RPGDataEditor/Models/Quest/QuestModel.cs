using System.Collections.Generic;

namespace RPGDataEditor.Models
{
    public class QuestModel : IdentifiableData
    {
        public IList<QuestTask> Tasks { get; private set; }

        public QuestTask CompletionTask { get; set; }
    }
}
