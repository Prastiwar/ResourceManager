using RPGDataEditor.Core.Models;
using RPGDataEditor.Wpf.Views;

namespace RPGDataEditor.Wpf
{
    public static class ViewsExtensions
    {
        public static PlayerRequirementModel CreateRequirement(this RequirementView.ChangeTypeEventArgs e)
        {
            bool isDialogue = string.Compare(e.TargetType, "dialogue", true) == 0;
            bool isQuest = string.Compare(e.TargetType, "quest", true) == 0;
            bool isItem = string.Compare(e.TargetType, "item", true) == 0;
            PlayerRequirementModel newModel = null;
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
            return newModel;
        }
    }
}
