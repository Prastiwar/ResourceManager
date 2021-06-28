using ResourceManager.Core.Serialization;
using RpgDataEditor.Models;

namespace RpgDataEditor.Serialization
{
    public class QuestTaskJsonConverter : AbstractClassJsonConverter<IQuestTask>
    {
        public QuestTaskJsonConverter() : base(new[] { typeof(IQuestTask).Assembly }) { }

        protected override string Suffix => "QuestTask";
    }
}
