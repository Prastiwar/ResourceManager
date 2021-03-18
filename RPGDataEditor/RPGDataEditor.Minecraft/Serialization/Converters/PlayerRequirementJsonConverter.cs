using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Serialization;

namespace RPGDataEditor.Minecraft.Serialization
{
    public class PlayerRequirementJsonConverter : AbstractClassJsonConverter<PlayerRequirementModel>
    {
        public PlayerRequirementJsonConverter() : base("RPGDataEditor.Minecraft.Models, RPGDataEditor.Core.Models") { }

        protected override string Suffix => "Requirement";
    }
}
