namespace RPGDataEditor.Core.Models
{
    public class SimpleCategorizedData : SimpleIdentifiableData
    {
        private string category = "";
        public string Category {
            get => category;
            set => SetProperty(ref category, value ?? "");
        }
    }
}
