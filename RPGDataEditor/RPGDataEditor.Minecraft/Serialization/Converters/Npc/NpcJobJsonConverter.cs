using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Serialization;

namespace RPGDataEditor.Minecraft.Serialization
{
    public class NpcJobJsonConverter : AbstractClassJsonConverter<NpcJobModel>
    {
        public NpcJobJsonConverter() : base(new[] { typeof(NpcJobJsonConverter).Assembly, typeof(NpcJobModel).Assembly }, 
                                            "RPGDataEditor.Minecraft.Models", "RPGDataEditor.Core.Models") { }

        protected override string Suffix => "NpcJobModel";
    }
}
