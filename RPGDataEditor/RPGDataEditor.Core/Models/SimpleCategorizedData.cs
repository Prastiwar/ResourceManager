namespace RPGDataEditor.Core.Models
{
    public class SimpleCategorizedData : SimpleIdentifiableData
    {
        private string category = "Uncategorized";
        public string Category {
            get => category;
            set => SetProperty(ref category, string.IsNullOrWhiteSpace(value) ? "Uncategorized" : value);
        }
    }
}
