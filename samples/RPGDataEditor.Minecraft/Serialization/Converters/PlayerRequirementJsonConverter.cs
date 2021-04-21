using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Serialization;

namespace RPGDataEditor.Minecraft.Serialization
{
    public class PlayerRequirementJsonConverter : AbstractClassJsonConverter<PlayerRequirementModel>
    {
        public PlayerRequirementJsonConverter() : base(new[] { typeof(PlayerRequirementJsonConverter).Assembly, typeof(PlayerRequirementModel).Assembly },
                                                        "RPGDataEditor.Minecraft.Models", "RPGDataEditor.Core.Models")
        { }

        protected override string Suffix => "Requirement";
    }
}
