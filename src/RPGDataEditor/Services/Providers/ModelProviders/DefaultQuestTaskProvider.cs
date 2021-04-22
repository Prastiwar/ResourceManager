using RPGDataEditor.Models;

namespace RPGDataEditor.Providers
{
    public class DefaultQuestTaskProvider : DefaultModelProvider<IQuestTask>
    {
        protected override string GetTypeNameToCompare(string name) => name.Replace("QuestTask", "");
    }
}