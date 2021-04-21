using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RPGDataEditor.Mvvm.Models
{
    public class NpcDataModel : ObservableModel, IIdentifiable
    {
        object IIdentifiable.Id {
            get => Id;
            set => Id = (int)value;
        }

        private int id = -1;
        public int Id {
            get => id;
            set => SetProperty(ref id, value);
        }

        private string name = "";
        public string Name {
            get => name;
            set => SetProperty(ref name, value ?? "");
        }

        private Position position = new Position();
        public Position Position {
            get => position;
            set => SetProperty(ref position, value ?? new Position());
        }

        private TalkDataModel talkData = new TalkDataModel();
        public TalkDataModel TalkData {
            get => talkData;
            set => SetProperty(ref talkData, value ?? new TalkDataModel());
        }

        private NpcJobModel job;
        public NpcJobModel Job {
            get => job;
            set => SetProperty(ref job, value);
        }

        private IList<AttributeDataModel> attributes = new ObservableCollection<AttributeDataModel>();
        public IList<AttributeDataModel> Attributes {
            get => attributes;
            set => SetProperty(ref attributes, value ?? new ObservableCollection<AttributeDataModel>());
        }
    }
}
