namespace RPGDataEditor.Minecraft.Models
{
    public class DialogueOptionModel : Core.Models.DialogueOptionModel
    {
        private string command = "";
        public string Command {
            get => command;
            set => SetProperty(ref command, value ?? "");
        }

        private int color = 1;
        public int Color {
            get => color;
            set => SetProperty(ref color, value);
        }
    }
}
