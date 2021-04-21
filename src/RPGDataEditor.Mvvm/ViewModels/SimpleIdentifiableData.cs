using System;

namespace RPGDataEditor.Mvvm.Models
{
    public class SimpleIdentifiableData : ObservableModel, IIdentifiable
    {
        public SimpleIdentifiableData(Type realType) => RealType = realType;

        public Type RealType { get; }

        object IIdentifiable.Id {
            get => Id;
            set => Id = (int)value;
        }

        private int id = -1;
        public int Id {
            get => id;
            set {
                if (SetProperty(ref id, value))
                {
                    OnPropertyChanged(nameof(RepresentableString));
                }
            }
        }

        private string name = "";
        public string Name {
            get => name;
            set {
                if (SetProperty(ref name, value ?? ""))
                {
                    OnPropertyChanged(nameof(RepresentableString));
                }
            }
        }

        public string RepresentableString => $"(ID: {Id}) {Name}";

        public override string ToString() => RepresentableString;
    }
}
