using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics.CodeAnalysis;

namespace RPGDataEditor.Core.Serialization
{
    public class ConfigurationSectionJsonConverter : JsonConverter<IConfigurationSection>
    {
        public override IConfigurationSection ReadJson(JsonReader reader, Type objectType, [AllowNull] IConfigurationSection existingValue, bool hasExistingValue, JsonSerializer serializer)
            => throw new NotSupportedException($"Reading to {nameof(IConfigurationSection)} is not supported");

        public override void WriteJson(JsonWriter writer, [AllowNull] IConfigurationSection value, JsonSerializer serializer)
        {
            JObject rootObject = GetSectionJObject(value, serializer);
            rootObject.WriteTo(writer);
        }

        protected virtual JObject GetSectionJObject(IConfigurationSection value, JsonSerializer serializer)
        {
            JObject rootObject = new JObject();
            foreach (IConfigurationSection section in value.GetChildren())
            {
                if (section.Value == null)
                {
                    JObject sectionObject = GetSectionJObject(section, serializer);
                    rootObject.Add(sectionObject.ToString());
                }
                else
                {
                    rootObject.Add(section.Key, section.Value);
                }
            }
            return new JObject() {
                { value.Key, rootObject }
            };
        }
    }
}
