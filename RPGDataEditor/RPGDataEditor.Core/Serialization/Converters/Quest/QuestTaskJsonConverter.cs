using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core.Serialization
{
    public class QuestTaskJsonConverter : AbstractClassJsonConverter<QuestTask>
    {
        public QuestTaskJsonConverter() : base(new[] { typeof(QuestTask).Assembly }, "RPGDataEditor.Core.Models") { }

        protected override string Suffix => "QuestTask";
    }
}
