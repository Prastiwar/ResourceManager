using RPGDataEditor.Core.Serialization;
using RPGDataEditor.Sample.Models;

namespace RPGDataEditor.Sample.Serialization
{
    public class QuestTaskJsonConverter : AbstractClassJsonConverter<IQuestTask>
    {
        public QuestTaskJsonConverter() : base(new[] { typeof(IQuestTask).Assembly }, "RPGDataEditor.Sample.Models") { }

        protected override string Suffix => "QuestTask";
    }
}
