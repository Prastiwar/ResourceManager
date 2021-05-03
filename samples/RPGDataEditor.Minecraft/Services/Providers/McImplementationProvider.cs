using ResourceManager;
using RPGDataEditor.Models;
using RPGDataEditor.Providers;
using System;
using System.Collections.Generic;

namespace RPGDataEditor.Minecraft.Providers
{
    public class McImplementationProvider<T> : DefaultImplementationProvider<T> where T : class
    {
        public McImplementationProvider(IFluentAssemblyScanner scanner) : base(scanner) { }

        protected override HashSet<Type> GetIgnoredTypes() => new HashSet<Type>() {
            typeof(Npc),
            typeof(Dialogue),
            typeof(DialogueOption),
            typeof(TalkData),
            typeof(ItemRequirement),
            typeof(RightItemInteractQuestTask)
        };
    }
}
