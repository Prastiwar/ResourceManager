using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RPGDataEditor.Core.Models
{
    public class IdentifiableData : ObservableModel, IIdentifiable
    {
        private int id;
        public int GetId() => id;
        public void SetId(int value) => id = value;

        private string title = "";
        public string Title {
            get => title;
            set => SetProperty(ref title, value ?? "");
        }

        private string message = "";
        public string Message {
            get => message;
            set => SetProperty(ref message, value ?? "");
        }

        private string category = "";
        public string Category {
            get => category;
            set => SetProperty(ref category, value ?? "");
        }

        private IList<PlayerRequirementModel> requirements = new ObservableCollection<PlayerRequirementModel>();
        public IList<PlayerRequirementModel> Requirements {
            get => requirements;
            set => SetProperty(ref requirements, value ?? new ObservableCollection<PlayerRequirementModel>());
        }

        public override string ToString() => $"(ID: {GetId()}) {Title}";
    }
}
