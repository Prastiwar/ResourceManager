using RPGDataEditor.Core.Models;
using System;

namespace RPGDataEditor.Core.Serialization
{
    public class QuestTaskJsonConverter : AbstractClassJsonConverter<QuestTask>
    {
        public QuestTaskJsonConverter() : base("RPGDataEditor.Core.Models") { }

        protected override string GetTypeName(QuestTask src) => base.GetTypeName(src).Replace("QuestTask", "");

        protected override Type GetObjectType(string type) => Type.GetType(namespaceName + "." + type + "QuestTask");
    }
}
