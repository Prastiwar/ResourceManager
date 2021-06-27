namespace RpgDataEditor.Models
{
    public class AttributeData
    {
        public AttributeData() { }
        public AttributeData(string name, double value = 0.0)
        {
            Name = name;
            Value = value;
        }

        private string name = "";
        public string Name {
            get => name;
            set => name = value ?? "";
        }

        public double Value { get; set; }
    }
}
