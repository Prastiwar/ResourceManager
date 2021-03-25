using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core.Providers
{
    public class DefaultQuestTaskProvider : DefaultModelProvider<QuestTask>
    {
        protected override string GetTypeNameToCompare(string name) => name.Replace("QuestTask", "");
    }
}