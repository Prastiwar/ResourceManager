namespace RPGDataEditor.Minecraft.Models
{
    public class DialogueModel : Core.Models.DialogueModel
    {
        private bool allowEscape = false;
        public bool AllowEscape {
            get => allowEscape;
            set => SetProperty(ref allowEscape, value);
        }
    }
}
