using RPGDataEditor.Core.Models;
using System;

namespace RPGDataEditor.Core.Connection
{
    public class DefaultResourceToTypeConverter : IResourceToTypeConverter
    {
        public Type GetResourceType(int resource) => (RPGResource)resource switch {
            RPGResource.Quest => typeof(QuestModel),
            RPGResource.Dialogue => typeof(DialogueModel),
            RPGResource.Npc => typeof(NpcDataModel),
            _ => throw new NotSupportedException(nameof(resource)),
        };

        public Type GetResourceType(IIdentifiable resource) => resource switch {
            QuestModel _ => typeof(QuestModel),
            DialogueModel _ => typeof(DialogueModel),
            NpcDataModel _ => typeof(NpcDataModel),
            SimpleCategorizedData categorizedData => categorizedData.RealType,
            SimpleIdentifiableData data => data.RealType,
            _ => throw new NotSupportedException(nameof(resource)),
        };
    }
}
