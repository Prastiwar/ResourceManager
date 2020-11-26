namespace RPGDataEditor.Core.Models
{
    public class TalkLine : ObservableModel
    {
        private string text;
        public string Text {
            get => text;
            set => SetProperty(ref text, value);
        }

        private string soundLocation;
        public string SoundLocation {
            get => soundLocation;
            set => SetProperty(ref soundLocation, value);
        }
    }
}
