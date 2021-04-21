//using RPGDataEditor.Models;
//using System;

//namespace RPGDataEditor.Core
//{
//    public class DefaultTypeToResourceConverter : ITypeToResourceConverter
//    {
//        public int ToResource(Type type) => type switch {
//            Type thisType when typeof(QuestModel).IsAssignableFrom(thisType) => (int)RPGResource.Quest,
//            Type thisType when typeof(DialogueModel).IsAssignableFrom(thisType) => (int)RPGResource.Dialogue,
//            Type thisType when typeof(NpcDataModel).IsAssignableFrom(thisType) => (int)RPGResource.Npc,
//            _ => throw new NotSupportedException(nameof(type)),
//        };
//    }
//}
