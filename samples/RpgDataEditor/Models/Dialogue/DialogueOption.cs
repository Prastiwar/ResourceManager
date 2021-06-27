using System.Collections.Generic;

namespace RpgDataEditor.Models
{
    public class DialogueOption
    {
        public string Message { get; set; }

        public object NextDialogId { get; set; }

        public IList<Requirement> Requirements { get; set; }
    }
}
