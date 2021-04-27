using RPGDataEditor.Models;

namespace RPGDataEditor.Providers
{
    public class DefaultQuestTaskProvider : DefaultImplementationProvider<IQuestTask>
    {
        protected override string GetTypeNameToCompare(string name) => name.Replace("QuestTask", "");
    }
}