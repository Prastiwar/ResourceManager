using System.Collections.Generic;

namespace RPGDataEditor.Core.Models
{
    public class PlayerDataModel : ObservableModel, IIdentifiable
    {
        private int id;
        public int GetId() => id;
        public void SetId(int value) => id = value;

        private string playerId = "";
        public string PlayerId {
            get => playerId;
            set => SetProperty(ref playerId, value ?? "");
        }

        private HashSet<int> history = new HashSet<int>();
        public HashSet<int> History {
            get => history;
            set => SetProperty(ref history, value ?? new HashSet<int>());
        }
    }
}
