using System;

namespace RPGDataEditor.Models
{
    public class SimpleIdentifiableData : IIdentifiable
    {
        public SimpleIdentifiableData(Type realType) => RealType = realType;

        public Type RealType { get; }

        public object Id { get; set; }

        public string Name { get; set; }

        public string RepresentableString => $"(ID: {Id}) {Name}";

        public override string ToString() => RepresentableString;
    }
}
