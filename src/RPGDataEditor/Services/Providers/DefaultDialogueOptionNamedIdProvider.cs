using RPGDataEditor.Models;
using System.Collections.Generic;
using System.Linq;

namespace RPGDataEditor.Providers
{
    public sealed class DefaultDialogueOptionNamedIdProvider : INamedIdProvider<DialogueOptionModel>
    {
        private List<KeyValuePair<string, int>> Set { get; } = new List<KeyValuePair<string, int>>();

        public DefaultDialogueOptionNamedIdProvider()
        {
            Add("Quit", -1);
            Add("Job", -2);
            Add("Dialogue", 0);
        }

        private void Add(string name, int id) => Set.Add(new KeyValuePair<string, int>(name, id));

        private int FixId(int id) => id > 0 ? 0 : id;

        public int GetId(string name) => Set.First(x => x.Key.CompareTo(name) == 0).Value;

        public string GetName(int id)
        {
            id = FixId(id);
            return Set.First(x => x.Value == id).Key;
        }
    }
}
