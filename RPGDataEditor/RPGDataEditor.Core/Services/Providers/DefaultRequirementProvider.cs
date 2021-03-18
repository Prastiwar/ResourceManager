using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core.Providers
{
    public class DefaultRequirementProvider : IRequirementProvider
    {
        public virtual PlayerRequirementModel CreateRequirement(string name)
        {
            PlayerRequirementModel newModel = null;
            bool isDialogue = string.Compare(name, "dialogue", true) == 0;
            bool isQuest = string.Compare(name, "quest", true) == 0;
            bool isItem = string.Compare(name, "item", true) == 0;
            bool isMoney = string.Compare(name, "money", true) == 0;
            if (isDialogue)
            {
                newModel = new DialogueRequirement();
            }
            else if (isQuest)
            {
                newModel = new QuestRequirement();
            }
            else if (isItem)
            {
                newModel = new ItemRequirement();
            }
            else if (isMoney)
            {
                newModel = new MoneyRequirement();
            }
            return newModel;
        }
    }
}