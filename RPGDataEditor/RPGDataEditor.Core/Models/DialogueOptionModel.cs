namespace RPGDataEditor.Core.Models
{
    public class DialogueOptionModel : ObservableModel
    {
        private string message = "";
        public string Message {
            get => message;
            set => SetProperty(ref message, value ?? "");
        }

        private string command = "";
        public string Command {
            get => message;
            set => SetProperty(ref command, value ?? "");
        }

        private int nextDialogId = -1;
        public int NextDialogId {
            get => nextDialogId;
            set => SetProperty(ref nextDialogId, value);
        }

        private int color = 1;
        public int Color {
            get => color;
            set => SetProperty(ref color, value);
        }

        private PlayerRequirementModel[] requirements = new PlayerRequirementModel[0];
        public PlayerRequirementModel[] Requirements {
            get => requirements;
            set => SetProperty(ref requirements, value ?? new PlayerRequirementModel[0]);
        }
    }
}
