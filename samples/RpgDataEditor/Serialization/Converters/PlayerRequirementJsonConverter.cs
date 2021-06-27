using ResourceManager.Core.Serialization;
using RpgDataEditor.Models;

namespace RpgDataEditor.Serialization
{
    public class PlayerRequirementJsonConverter : AbstractClassJsonConverter<Requirement>
    {
        public PlayerRequirementJsonConverter() : base(new[] { typeof(Requirement).Assembly }, "RpgDataEditor.Models") { }

        protected override string Suffix => "Requirement";
    }
}
