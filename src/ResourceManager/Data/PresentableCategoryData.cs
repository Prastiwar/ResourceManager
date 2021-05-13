using System;

namespace ResourceManager.Data
{
    public class PresentableCategoryData : PresentableData
    {
        public PresentableCategoryData(Type presentingType) : base(presentingType) { }

        public string Category { get; set; }
    }
}
