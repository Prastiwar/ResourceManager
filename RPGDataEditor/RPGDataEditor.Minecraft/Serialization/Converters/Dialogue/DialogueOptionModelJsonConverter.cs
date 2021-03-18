using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Minecraft.Models;
using RPGDataEditor.Core;
using System;

namespace RPGDataEditor.Minecraft.Serialization
{
    public class DialogueOptionModelJsonConverter : Core.Serialization.DialogueOptionModelJsonConverter
    {
        public override Core.Models.DialogueOptionModel ReadJObject(Type objectType, JObject obj)
        {
            Core.Models.DialogueOptionModel coreModel = base.ReadJObject(objectType, obj);
            int color = obj.GetValue<int>(nameof(DialogueOptionModel.Color), 0);
            string command = obj.GetValue<string>(nameof(DialogueOptionModel.Command));
            DialogueOptionModel model = new DialogueOptionModel() {
                NextDialogId = coreModel.NextDialogId,
                Message = coreModel.Message,
                Command = command,
                Color = color,
                Requirements = coreModel.Requirements
            };
            return model;
        }

        public override JObject ToJObject(Core.Models.DialogueOptionModel value, JsonSerializer serializer)
        {
            JObject obj = base.ToJObject(value, serializer);
            obj.Add(nameof(DialogueOptionModel.Command).ToFirstLower(), value.Command);
            obj.Add(nameof(DialogueOptionModel.Color).ToFirstLower(), value.Color);
            return obj;
        }
    }
}
