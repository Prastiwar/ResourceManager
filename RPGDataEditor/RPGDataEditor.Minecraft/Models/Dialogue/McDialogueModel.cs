using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Minecraft.Models
{
    public class McDialogueModel : DialogueModel
    {
        private bool allowEscape = false;
        public bool AllowEscape {
            get => allowEscape;
            set => SetProperty(ref allowEscape, value);
        }
    }
}
