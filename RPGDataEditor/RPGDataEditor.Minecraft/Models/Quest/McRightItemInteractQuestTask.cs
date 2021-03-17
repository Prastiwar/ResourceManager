using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Minecraft.Models
{
    public class McRightItemInteractQuestTask : RightItemInteractQuestTask
    {
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
