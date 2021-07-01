using ResourceManager.Data;
using System.Collections.Generic;

namespace RpgDataEditor.Models
{
    public class Quest : ICategorizable
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public string Category { get; set; }

        public IQuestTask CompletionTask { get; set; }

        public IList<IQuestTask> Tasks { get; set; }

        public IList<Requirement> Requirements { get; set; }

        object IIdentifiable.Id {
            get => Id;
            set => Id = (int)System.Convert.ToInt64(value);
        }
    }
}
