using RPGDataEditor.Core.Serialization;
using RPGDataEditor.Models;

namespace RPGDataEditor.Minecraft.Serialization
{
    public class PlayerRequirementJsonConverter : AbstractClassJsonConverter<Requirement>
    {
        public PlayerRequirementJsonConverter() : base(new[] { typeof(PlayerRequirementJsonConverter).Assembly, typeof(Requirement).Assembly },
                                                        "RPGDataEditor.Minecraft.Models", "RPGDataEditor.Models")
        { }

        protected override string Suffix => "Requirement";
    }
}
