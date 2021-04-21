using System.Collections.Generic;

namespace RPGDataEditor.Models
{
    public class DialogueOptionModel
    {
        private string message = "";
        public string Message {
            get => message;
            set => message = value ?? "";
        }

        public object NextDialogId { get; set; }

        public IList<PlayerRequirementModel> Requirements { get; private set; }
    }
}
