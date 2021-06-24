using RPGDataEditor.Core.Serialization;
using RPGDataEditor.Sample.Models;

namespace RPGDataEditor.Sample.Serialization
{
    public class NpcJobJsonConverter : AbstractClassJsonConverter<NpcJob>
    {
        public NpcJobJsonConverter() : base(new[] { typeof(NpcJob).Assembly }, "RPGDataEditor.Sample.Models") { }

        protected override string Suffix => "NpcJob";
    }
}
