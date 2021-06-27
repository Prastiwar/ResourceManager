using RPGDataEditor.Models;

namespace RPGDataEditor.Minecraft.Models
{
    public class RightItemInteractQuestTask : ItemInteractQuestTask
    {
        public bool RespectNbt { get; set; }

        public string Nbt { get; set; }
    }
}
