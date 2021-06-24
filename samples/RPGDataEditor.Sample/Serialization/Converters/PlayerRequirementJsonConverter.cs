using RPGDataEditor.Core.Serialization;
using RPGDataEditor.Sample.Models;

namespace RPGDataEditor.Sample.Serialization
{
    public class PlayerRequirementJsonConverter : AbstractClassJsonConverter<Requirement>
    {
        public PlayerRequirementJsonConverter() : base(new[] { typeof(Requirement).Assembly }, "RPGDataEditor.Sample.Models") { }

        protected override string Suffix => "Requirement";
    }
}
