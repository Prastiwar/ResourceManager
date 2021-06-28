using ResourceManager.Core.Serialization;
using RpgDataEditor.Models;

namespace RpgDataEditor.Serialization
{
    public class PlayerRequirementJsonConverter : AbstractClassJsonConverter<Requirement>
    {
        public PlayerRequirementJsonConverter() : base(new[] { typeof(Requirement).Assembly }) { }

        protected override string Suffix => "Requirement";
    }
}
