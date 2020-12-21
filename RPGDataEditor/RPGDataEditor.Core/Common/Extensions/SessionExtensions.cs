using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;
using System.Threading.Tasks;

namespace RPGDataEditor.Core
{
    public static class SessionExtensions
    {
        public static Task<QuestModel[]> LoadQuests(this SessionContext session) => session.LoadAsync<QuestModel>("quests");
        public static Task<DialogueModel[]> LoadDialogues(this SessionContext session) => session.LoadAsync<DialogueModel>("dialogues");
    }
}
