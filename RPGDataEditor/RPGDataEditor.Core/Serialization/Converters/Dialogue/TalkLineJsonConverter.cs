using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Core.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace RPGDataEditor.Core.Serialization
{
    public class TalkLineJsonConverter : JsonConverter<TalkLine>
    {
        public override TalkLine ReadJson(JsonReader reader, Type objectType, [AllowNull] TalkLine existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject obj = JObject.Load(reader);
                string text = obj.GetValue<string>(nameof(TalkLine.Text));
                string soundLocation = obj.GetValue<string>(nameof(TalkLine.SoundLocation));
                TalkLine model = new TalkLine() {
                    Text = text,
                    SoundLocation = soundLocation
                };
                return model;
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] TalkLine value, JsonSerializer serializer)
        {
            JObject obj = new JObject() {
                { nameof(TalkLine.Text).ToFirstLower(), value.Text },
                { nameof(TalkLine.SoundLocation).ToFirstLower(), JArray.FromObject(value.SoundLocation) }
            };
            obj.WriteTo(writer);
        }
    }
}
