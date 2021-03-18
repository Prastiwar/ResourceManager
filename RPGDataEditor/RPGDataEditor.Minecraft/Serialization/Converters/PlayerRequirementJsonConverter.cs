using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core.Serialization
{
    public class PlayerRequirementJsonConverter : AbstractClassJsonConverter<PlayerRequirementModel>
    {
        public PlayerRequirementJsonConverter() : base("RPGDataEditor.Minecraft.Models, RPGDataEditor.Core.Models") { }

        protected override string Suffix => "Requirement";
    }
}
