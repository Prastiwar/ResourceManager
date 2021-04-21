using RPGDataEditor.Models;

namespace RPGDataEditor.Providers
{
    public class DefaultQuestTaskProvider : DefaultModelProvider<QuestTask>
    {
        protected override string GetTypeNameToCompare(string name) => name.Replace("QuestTask", "");
    }
}