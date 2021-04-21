namespace RPGDataEditor.Models
{
    public class TalkLine
    {
        private string text = "";
        public string Text {
            get => text;
            set => text = value ?? "";
        }

        public object SoundId { get; set; }
    }
}
