using RPGDataEditor.Core.Models;
using System;

namespace RPGDataEditor.Core.Connection
{
    /// <summary>
    /// Converts Identifiable resource to relative paths following this structure
    /// --root
    /// --dialogues
    /// ----category name
    /// ------id_name.json
    /// ----other category name
    /// ------id_name.json
    /// --quests
    /// ----id_name.json
    /// --npcs
    /// ----id_name.json
    /// </summary>
    public class DefaultResourceToPathConverter : IResourceToPathConverter
    {
        public string ToRelativeRoot(int resource) => (RPGResource)resource switch {
            RPGResource.Quest => "quests",
            RPGResource.Dialogue => "dialogues",
            RPGResource.Npc => "npcs",
            _ => throw new NotSupportedException(nameof(resource)),
        };

        public string ToRelativeRoot(IIdentifiable resource) => resource switch {
            QuestModel _ => "quests",
            DialogueModel _ => "dialogues",
            NpcDataModel _ => "npcs",
            _ => throw new NotSupportedException(nameof(resource)),
        };

        public string ToRelativePath(IIdentifiable resource) => resource switch {
            QuestModel quest => $"quests/{quest.Category}/{quest.Id}_{quest.Title}.json",
            DialogueModel dialogue => $"dialogues/{dialogue.Category}/{dialogue.Id}_{dialogue.Title}.json",
            NpcDataModel npc => $"npcs/{npc.Id}_{npc.Name}",
            SimpleCategorizedData categorizedData => $"{ToRelativeRoot(categorizedData)}/{categorizedData.Category}/{categorizedData.Id}_{categorizedData.Name}.json",
            SimpleIdentifiableData data => $"{ToRelativeRoot(data)}/{data.Id}_{data.Name}.json",
            _ => throw new NotSupportedException(nameof(resource)),
        };

        private string ToRelativeRoot(SimpleIdentifiableData data) => data.RealType switch {
            Type realType when typeof(QuestModel).IsAssignableFrom(realType) => "quests",
            Type realType when typeof(DialogueModel).IsAssignableFrom(realType) => "dialogues",
            Type realType when typeof(NpcDataModel).IsAssignableFrom(realType) => "npcs",
            _ => throw new NotSupportedException(nameof(data)),
        };
    }
}
