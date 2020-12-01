using Newtonsoft.Json;
using RPGDataEditor.Core.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace RPGDataEditor.Core.Serialization
{
    public class PlayerRequirementJsonConverter : AbstractClassJsonConverter<PlayerRequirementModel>
    {
        public PlayerRequirementJsonConverter() : base("RPGDataEditor.Core.Models") { }

        public override void WriteJson(JsonWriter writer, [AllowNull] PlayerRequirementModel value, JsonSerializer serializer)
        {
            if (value is PlayerRequirementBuilder builder)
            {
                value = builder.Model;
            }
            base.WriteJson(writer, value, serializer);
        }

        protected override string GetTypeName(PlayerRequirementModel src) => base.GetTypeName(src).Replace("Requirement", "");

        protected override Type GetObjectType(string type) => Type.GetType(namespaceName + "." + type + "Requirement");
    }
}
