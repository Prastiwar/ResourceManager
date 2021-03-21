using System;

namespace RPGDataEditor.Core.Models
{
    public class SimpleCategorizedData : SimpleIdentifiableData
    {
        public SimpleCategorizedData(Type realType) : base(realType) { }

        private string category = "Uncategorized";
        public string Category {
            get => category;
            set => SetProperty(ref category, string.IsNullOrWhiteSpace(value) ? "Uncategorized" : value);
        }
    }
}
