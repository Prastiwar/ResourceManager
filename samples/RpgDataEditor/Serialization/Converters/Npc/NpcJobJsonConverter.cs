using ResourceManager.Core.Serialization;
using RpgDataEditor.Models;

namespace RpgDataEditor.Serialization
{
    public class NpcJobJsonConverter : AbstractClassJsonConverter<NpcJob>
    {
        public NpcJobJsonConverter() : base(new[] { typeof(NpcJob).Assembly }, "RpgDataEditor.Models") { }

        protected override string Suffix => "NpcJob";
    }
}
