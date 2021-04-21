using System.Collections.Generic;

namespace RPGDataEditor.Models
{
    public class NpcDataModel : IIdentifiable
    {
        public object Id { get; set; }

        private string name = "";
        public string Name {
            get => name;
            set => name = value ?? "";
        }

        public Position Position { get; set; }

        public TalkDataModel TalkData { get; set; }

        public NpcJobModel Job { get; set; }

        public IList<AttributeDataModel> Attributes { get; private set; }
    }
}
