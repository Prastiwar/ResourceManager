using RPGDataEditor.Models;

namespace RPGDataEditor.Core.Serialization
{
    public class QuestTaskJsonConverter : AbstractClassJsonConverter<IQuestTask>
    {
        public QuestTaskJsonConverter() : base(new[] { typeof(IQuestTask).Assembly }, "RPGDataEditor.Models") { }

        protected override string Suffix => "QuestTask";
    }
}
