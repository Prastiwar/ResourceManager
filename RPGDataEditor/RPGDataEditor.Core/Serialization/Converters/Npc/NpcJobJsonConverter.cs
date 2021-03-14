using RPGDataEditor.Core.Models;
using System;

namespace RPGDataEditor.Core.Serialization
{
    public class NpcJobJsonConverter : AbstractClassJsonConverter<NpcJobModel>
    {
        public NpcJobJsonConverter() : base("RPGDataEditor.Core.Models") { }

        protected override string GetTypeName(NpcJobModel src) => base.GetTypeName(src).Replace("NpcJobModel", "");

        protected override Type GetObjectType(string type) => Type.GetType(namespaceName + "." + type + "NpcJobModel");
    }
}
