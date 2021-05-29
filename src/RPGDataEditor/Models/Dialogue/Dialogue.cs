using ResourceManager.Data;
using System.Collections.Generic;

namespace RPGDataEditor.Models
{
    public class Dialogue : IIdentifiable
    {
        public object Id { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public string Category { get; set; }

        public object StartQuestId { get; set; }

        public IList<Requirement> Requirements { get; set; }

        public IList<DialogueOption> Options { get; set; }
    }
}
