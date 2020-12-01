using RPGDataEditor.Core.Models;
using System;

namespace RPGDataEditor.Core.Serialization
{
    public class PlayerRequirementJsonConverter : AbstractClassJsonConverter<PlayerRequirementModel>
    {
        public PlayerRequirementJsonConverter() : base("RPGDataEditor.Core.Models") { }

        protected override string GetTypeName(PlayerRequirementModel src) => base.GetTypeName(src).Replace("Requirement", "");

        protected override Type GetObjectType(string type) => Type.GetType(namespaceName + "." + type + "Requirement");
    }
}
