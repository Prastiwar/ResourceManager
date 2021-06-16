using System;

namespace ResourceManager.Data
{
    public class PresentableCategoryData : PresentableData, ICategorizable
    {
        public PresentableCategoryData(Type presentingType) : base(presentingType) { }

        public string Category { get; set; }
    }
}
