using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Providers;
using System;
using System.Collections.Generic;

namespace RPGDataEditor.Minecraft.Providers
{
    public class McModelProvider<TModel> : DefaultModelProvider<TModel> where TModel : ObservableModel
    {
        protected override HashSet<Type> GetIgnoredTypes() => new HashSet<Type>() {
            typeof(NpcDataModel),
            typeof(DialogueModel),
            typeof(DialogueOptionModel),
            typeof(TalkDataModel),
            typeof(ItemRequirement),
            typeof(RightItemInteractQuestTask)
        };
    }
}
