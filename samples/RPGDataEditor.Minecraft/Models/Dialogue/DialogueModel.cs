namespace RPGDataEditor.Minecraft.Models
{
    public class Dialogue : RPGDataEditor.Models.Dialogue
    {
        private bool allowEscape = false;
        public bool AllowEscape {
            get => allowEscape;
            set => SetProperty(ref allowEscape, value);
        }
    }
}
