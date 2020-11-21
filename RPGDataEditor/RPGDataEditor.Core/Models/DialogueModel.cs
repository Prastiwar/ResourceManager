namespace RPGDataEditor.Core.Models
{
    public class DialogueModel : IdentifiableData
    {
        private int startQuest = -1;
        public int StartQuest {
            get => startQuest;
            set => SetProperty(ref startQuest, value);
        }

        private DialogueOptionModel[] options = new DialogueOptionModel[0];
        public DialogueOptionModel[] Options {
            get => options;
            set => SetProperty(ref options, value ?? new DialogueOptionModel[0]);
        }
    }
}
