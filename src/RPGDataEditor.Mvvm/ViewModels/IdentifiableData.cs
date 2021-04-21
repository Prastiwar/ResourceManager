using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RPGDataEditor.Mvvm.Models
{
    public class IdentifiableData : ObservableModel, IIdentifiable
    {
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

        private string title = "";
        public string Title {
            get => title;
            set {
                if (SetProperty(ref title, value ?? ""))
                {
                    OnPropertyChanged(nameof(RepresentableString));
                }
            }
        }

        private string message = "";
        public string Message {
            get => message;
            set => SetProperty(ref message, value ?? "");
        }

        private string category = "Uncategorized";
        public string Category {
            get => category;
            set => SetProperty(ref category, string.IsNullOrWhiteSpace(value) ? "Uncategorized" : value);
        }

        private IList<PlayerRequirementModel> requirements = new ObservableCollection<PlayerRequirementModel>();
        public IList<PlayerRequirementModel> Requirements {
            get => requirements;
            set => SetProperty(ref requirements, value ?? new ObservableCollection<PlayerRequirementModel>());
        }

        public string RepresentableString => $"(ID: {Id}) {Title}";

        public override string ToString() => RepresentableString;
    }
}
