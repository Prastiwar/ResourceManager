using RPGDataEditor.Models;

namespace RPGDataEditor.Core.Serialization
{
    public class PlayerRequirementJsonConverter : AbstractClassJsonConverter<Requirement>
    {
        public PlayerRequirementJsonConverter() : base(new[] { typeof(Requirement).Assembly }, "RPGDataEditor.Models") { }

        protected override string Suffix => "Requirement";
    }
}
