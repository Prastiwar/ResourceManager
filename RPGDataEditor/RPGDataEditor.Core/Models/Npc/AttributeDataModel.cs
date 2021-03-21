namespace RPGDataEditor.Core.Models
{
    public class AttributeDataModel : ObservableModel
    {
        public AttributeDataModel() {}
        public AttributeDataModel(string name, double value)
        {
            Name = name;
            Value = value;
        }

        private string name = "";
        public string Name {
            get => name;
            set => SetProperty(ref name, value);
        }

        private double value;
        public double Value {
            get => value;
            set => SetProperty(ref this.value, value);
        }
    }
}
