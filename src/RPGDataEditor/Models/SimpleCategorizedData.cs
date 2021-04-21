using System;

namespace RPGDataEditor.Models
{
    public class SimpleCategorizedData : SimpleIdentifiableData
    {
        public SimpleCategorizedData(Type realType) : base(realType) { }

        private string category = "Uncategorized";
        public string Category {
            get => category;
            set => category = !string.IsNullOrWhiteSpace(value) ? value : "Uncategorized";
        }
    }
}
