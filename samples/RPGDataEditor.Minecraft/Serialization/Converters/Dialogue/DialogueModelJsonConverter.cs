using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Minecraft.Models;
using RPGDataEditor.Core;
using System;

namespace RPGDataEditor.Minecraft.Serialization
{
    public class DialogueJsonConverter : Core.Serialization.DialogueJsonConverter
    {
        public override RPGDataEditor.Models.Dialogue ReadJObject(Type objectType, JObject obj)
        {
            RPGDataEditor.Models.Dialogue coreModel = base.ReadJObject(objectType, obj);
            bool allowEscape = obj.GetValue(nameof(Dialogue.AllowEscape), false);
            Dialogue model = new Dialogue() {
                Id = coreModel.Id,
                Title = coreModel.Title,
                Message = coreModel.Message,
                Category = coreModel.Category,
                AllowEscape = allowEscape,
                StartQuest = coreModel.StartQuest,
                Requirements = coreModel.Requirements,
                Options = coreModel.Options
            };
            return model;
        }

        public override JObject ToJObject(RPGDataEditor.Models.Dialogue value, JsonSerializer serializer)
        {
            JObject obj = base.ToJObject(value, serializer);
            obj.Add(nameof(Dialogue.AllowEscape).ToFirstLower(), (value as Dialogue).AllowEscape);
            return obj;
        }
    }
}
