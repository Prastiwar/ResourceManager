using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Minecraft.Providers
{
    public class QuestTaskProvider : McModelProvider<QuestTask>
    {
        protected override string GetTypeNameToCompare(string name) => name.Replace("QuestTask", "");
    }
}