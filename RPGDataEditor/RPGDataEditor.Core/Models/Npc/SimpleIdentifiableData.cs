﻿namespace RPGDataEditor.Core.Models
{
    public class SimpleIdentifiableData : ObservableModel, IIdentifiable
    {
        private int id;
        public int Id {
            get => id;
            set {
                if (SetProperty(ref id, value))
                {
                    RaisePropertyChanged(nameof(RepresentableString));
                }
            }
        }

        private string name = "";
        public string Name {
            get => name;
            set {
                if (SetProperty(ref name, value ?? ""))
                {
                    RaisePropertyChanged(nameof(RepresentableString));
                }
            }
        }

        public string RepresentableString => $"(ID: {Id}) {Name}";

        public override string ToString() => RepresentableString;
    }
}