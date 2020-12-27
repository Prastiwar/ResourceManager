namespace RPGDataEditor.Core.Models
{
    public class ItemRequirement : PlayerRequirementModel
    {
        private string item;
        public string Item {
            get => item;
            set => SetProperty(ref item, value);
        }

        private bool respectNbt;
        public bool RespectNbt {
            get => respectNbt;
            set => SetProperty(ref respectNbt, value);
        }

        private string nbt;
        public string Nbt {
            get => nbt;
            set => SetProperty(ref nbt, value);
        }
    }
}
