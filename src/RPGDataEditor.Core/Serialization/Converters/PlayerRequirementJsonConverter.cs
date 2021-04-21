using RPGDataEditor.Models;

namespace RPGDataEditor.Core.Serialization
{
    public class PlayerRequirementJsonConverter : AbstractClassJsonConverter<PlayerRequirementModel>
    {
        public PlayerRequirementJsonConverter() : base(new[] { typeof(PlayerRequirementModel).Assembly }, "RPGDataEditor.Models") { }

        protected override string Suffix => "Requirement";
    }
}
