using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Serialization;

namespace RPGDataEditor.Minecraft.Serialization
{
    public class QuestTaskJsonConverter : AbstractClassJsonConverter<QuestTask>
    {
        public QuestTaskJsonConverter() : base(new[] { typeof(QuestTaskJsonConverter).Assembly, typeof(QuestTask).Assembly },
                                                "RPGDataEditor.Minecraft.Models", "RPGDataEditor.Core.Models")
        { }

        protected override string Suffix => "QuestTask";
    }
}
