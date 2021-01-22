using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;
using System;
using System.Threading.Tasks;

namespace RPGDataEditor.Core
{
    public static class SessionExtensions
    {
        public static Task<QuestModel[]> LoadQuests(this SessionContext session) => session.LoadAsync<QuestModel>("quests");
        public static Task<DialogueModel[]> LoadDialogues(this SessionContext session) => session.LoadAsync<DialogueModel>("dialogues");
        public static Task<NpcDataModel[]> LoadNpcs(this SessionContext session) => session.LoadAsync<NpcDataModel>("npcs");
    }
}
