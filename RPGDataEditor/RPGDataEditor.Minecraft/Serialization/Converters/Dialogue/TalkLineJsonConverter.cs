using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Minecraft.Models;
using RPGDataEditor.Core;
using System;

namespace RPGDataEditor.Minecraft.Serialization
{
    public class TalkLineJsonConverter : Core.Serialization.TalkLineJsonConverter
    {
        public override Core.Models.TalkLine ReadJObject(Type objectType, JObject obj)
        {
            Core.Models.TalkLine coreModel = base.ReadJObject(objectType, obj);
            string soundLocation = obj.GetValue<string>(nameof(TalkLine.SoundLocation));
            TalkLine model = new TalkLine() {
                Text = coreModel.Text,
                SoundLocation = soundLocation
            };
            return model;
        }

        public override JObject ToJObject(Core.Models.TalkLine value, JsonSerializer serializer)
        {
            JObject obj = base.ToJObject(value, serializer);
            obj.Add(nameof(TalkLine.SoundLocation).ToFirstLower(), value.SoundLocation);
            return obj;
        }
    }
}
