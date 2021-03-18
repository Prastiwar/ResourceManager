using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Core.Models;
using System;

namespace RPGDataEditor.Core.Serialization
{
    public class TalkLineJsonConverter : ExtendableJsonConverter<TalkLine>
    {
        public override TalkLine ReadJObject(Type objectType, JObject obj)
        {
            string text = obj.GetValue<string>(nameof(TalkLine.Text));
            string soundLocation = obj.GetValue<string>(nameof(TalkLine.SoundLocation));
            TalkLine model = new TalkLine() {
                Text = text,
                SoundLocation = soundLocation
            };
            return model;
        }

        public override JObject ToJObject(TalkLine value, JsonSerializer serializer) => new JObject() {
                { nameof(TalkLine.Text).ToFirstLower(), value.Text },
                { nameof(TalkLine.SoundLocation).ToFirstLower(), value.SoundLocation }
            };
    }
}
