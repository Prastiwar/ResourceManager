using ResourceManager.Core.Serialization;
using RpgDataEditor.Models;

namespace RpgDataEditor.Serialization
{
    public class QuestTaskJsonConverter : AbstractClassJsonConverter<QuestTask>
    {
        public QuestTaskJsonConverter() : base(new[] { typeof(QuestTask).Assembly }) { }

        protected override string Suffix => "QuestTask";
    }
}
