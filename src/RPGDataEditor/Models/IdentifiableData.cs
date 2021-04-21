using System.Collections.Generic;

namespace RPGDataEditor.Models
{
    public class IdentifiableData : IIdentifiable
    {
        public object Id { get; set; }

        private string title = "";
        public string Title {
            get => title;
            set => title = value ?? "";
        }

        private string message = "";
        public string Message {
            get => message;
            set => message = value ?? "";
        }

        private string category = "Uncategorized";
        public string Category {
            get => category;
            set => category = string.IsNullOrWhiteSpace(value) ? "Uncategorized" : value;
        }

        public IList<PlayerRequirementModel> Requirements { get; private set; }

        public string RepresentableString => $"(ID: {Id}) {Title}";

        public override string ToString() => RepresentableString;
    }
}
