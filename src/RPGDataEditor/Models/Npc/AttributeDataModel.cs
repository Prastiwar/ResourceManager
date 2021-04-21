namespace RPGDataEditor.Models
{
    public class AttributeDataModel
    {
        public AttributeDataModel() { }
        public AttributeDataModel(string name, double value = 0.0)
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
