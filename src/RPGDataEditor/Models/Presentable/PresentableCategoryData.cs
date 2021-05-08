using System;

namespace RPGDataEditor.Models
{
    public class PresentableCategoryData : PresentableData
    {
        public PresentableCategoryData(Type presentingType) : base(presentingType) { }

        public string Category { get; set; }
    }
}
