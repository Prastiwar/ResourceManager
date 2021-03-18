using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Minecraft.Models;
using RPGDataEditor.Core;
using System;

namespace RPGDataEditor.Minecraft.Serialization
{
    public class DialogueModelJsonConverter : Core.Serialization.DialogueModelJsonConverter
    {
        public override Core.Models.DialogueModel ReadJObject(Type objectType, JObject obj)
        {
            Core.Models.DialogueModel coreModel = base.ReadJObject(objectType, obj);
            bool allowEscape = obj.GetValue(nameof(DialogueModel.AllowEscape), false);
            DialogueModel model = new DialogueModel() {
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

        public override JObject ToJObject(Core.Models.DialogueModel value, JsonSerializer serializer)
        {
            JObject obj = base.ToJObject(value, serializer);
            obj.Add(nameof(DialogueModel.AllowEscape).ToFirstLower(), value.AllowEscape);
            return obj;
        }
    }
}
