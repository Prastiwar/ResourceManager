using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ResourceManager;
using RPGDataEditor.Core;
using System;

namespace RPGDataEditor.Minecraft.Serialization
{
    public class DialogueOptionJsonConverter : Core.Serialization.DialogueOptionJsonConverter
    {
        public override RPGDataEditor.Models.DialogueOption ReadJObject(Type objectType, JObject obj)
        {
            RPGDataEditor.Models.DialogueOption coreModel = base.ReadJObject(objectType, obj);
            int color = obj.GetValue<int>(nameof(Models.DialogueOption.Color), 0);
            string command = obj.GetValue<string>(nameof(Models.DialogueOption.Command));
            Models.DialogueOption model = new Models.DialogueOption() {
                NextDialogId = coreModel.NextDialogId,
                Message = coreModel.Message,
                Command = command,
                Color = color
            };
            model.Requirements.AddRange(coreModel.Requirements);
            return model;
        }

        public override JObject ToJObject(RPGDataEditor.Models.DialogueOption value, JsonSerializer serializer)
        {
            JObject obj = base.ToJObject(value, serializer);
            obj.Add(nameof(Models.DialogueOption.Command).ToFirstLower(), (value as Models.DialogueOption).Command);
            obj.Add(nameof(Models.DialogueOption.Color).ToFirstLower(), (value as Models.DialogueOption).Color);
            return obj;
        }
    }
}
