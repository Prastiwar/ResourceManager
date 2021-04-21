using RPGDataEditor.Models;

namespace RPGDataEditor.Core.Serialization
{
    public class NpcJobJsonConverter : AbstractClassJsonConverter<NpcJobModel>
    {
        public NpcJobJsonConverter() : base(new[] { typeof(NpcJobModel).Assembly }, "RPGDataEditor.Models") { }

        protected override string Suffix => "NpcJobModel";
    }
}
