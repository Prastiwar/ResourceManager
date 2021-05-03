using RPGDataEditor.Core.Serialization;
using RPGDataEditor.Models;

namespace RPGDataEditor.Minecraft.Serialization
{
    public class QuestTaskJsonConverter : AbstractClassJsonConverter<IQuestTask>
    {
        public QuestTaskJsonConverter() : base(new[] { typeof(QuestTaskJsonConverter).Assembly, typeof(IQuestTask).Assembly },
                                                "RPGDataEditor.Minecraft.Models", "RPGDataEditor.Models")
        { }

        protected override string Suffix => "QuestTask";
    }
}
