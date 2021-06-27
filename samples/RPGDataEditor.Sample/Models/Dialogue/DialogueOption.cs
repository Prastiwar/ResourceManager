using System.Collections.Generic;

namespace RPGDataEditor.Sample.Models
{
    public class DialogueOption
    {
        public string Message { get; set; }

        public object NextDialogId { get; set; }

        public IList<Requirement> Requirements { get; set; }
    }
}
