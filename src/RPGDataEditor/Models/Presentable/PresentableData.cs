using System;

namespace RPGDataEditor.Models
{
    public class PresentableData : IIdentifiable
    {
        public PresentableData(Type presentingType) => PresentingType = presentingType;

        public Type PresentingType { get; }

        public object Id { get; set; }

        public string Name { get; set; }

        public override string ToString() => $"(ID: {Id}) {Name}";

        public void Update(object obj)
        {
            if (PresentingType.IsAssignableFrom(obj.GetType()))
            {
                UpdateFromResource(obj);
            }
        }

        protected virtual void UpdateFromResource(object resource) { }
    }
}
