namespace RPGDataEditor.Minecraft.Models
{
    public class TalkLine : Core.Models.TalkLine
    {
        private string soundLocation;
        public string SoundLocation {
            get => soundLocation;
            set => SetProperty(ref soundLocation, value);
        }
    }
}
