using RPGDataEditor.Models;

namespace RPGDataEditor.Minecraft.Models
{
    public abstract class BlockInteractQuestTask : IQuestTask
    {
        public Position Pos { get; set; }
    }
}
