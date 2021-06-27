using RPGDataEditor.Core.Serialization;
using RPGDataEditor.Models;

namespace RPGDataEditor.Minecraft.Serialization
{
    public class NpcJobJsonConverter : AbstractClassJsonConverter<NpcJob>
    {
        public NpcJobJsonConverter() : base(new[] { typeof(NpcJobJsonConverter).Assembly, typeof(NpcJob).Assembly },
                                            "RPGDataEditor.Minecraft.Models", "RPGDataEditor.Models")
        { }

        protected override string Suffix => "NpcJob";
    }
}
