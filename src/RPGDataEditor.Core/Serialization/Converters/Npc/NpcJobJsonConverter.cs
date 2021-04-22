using RPGDataEditor.Models;

namespace RPGDataEditor.Core.Serialization
{
    public class NpcJobJsonConverter : AbstractClassJsonConverter<NpcJob>
    {
        public NpcJobJsonConverter() : base(new[] { typeof(NpcJob).Assembly }, "RPGDataEditor.Models") { }

        protected override string Suffix => "NpcJobModel";
    }
}
