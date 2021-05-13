using ResourceManager.Data;
using System.Collections.Generic;

namespace RPGDataEditor.Models
{
    public class Quest : IIdentifiable
    {
        public object Id { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public string Category { get; set; }

        public IQuestTask CompletionTask { get; set; }

        public IList<IQuestTask> Tasks { get; private set; }

        public IList<Requirement> Requirements { get; private set; }
    }
}
