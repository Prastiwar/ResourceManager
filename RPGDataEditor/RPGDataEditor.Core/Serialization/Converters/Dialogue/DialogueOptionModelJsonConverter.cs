using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RPGDataEditor.Core.Serialization
{
    public class DialogueOptionModelJsonConverter : ExtendableJsonConverter<DialogueOptionModel>
    {
        public override DialogueOptionModel ReadJObject(Type objectType, JObject obj)
        {
            int nextDialogId = obj.GetValue(nameof(DialogueOptionModel.NextDialogId), -1);
            int color = obj.GetValue<int>(nameof(DialogueOptionModel.Color), 0);
            string command = obj.GetValue<string>(nameof(DialogueOptionModel.Command));
            string message = obj.GetValue<string>(nameof(DialogueOptionModel.Message));
            IList<PlayerRequirementModel> requirements = obj.GetValue<ObservableCollection<PlayerRequirementModel>>(nameof(DialogueOptionModel.Requirements));
            DialogueOptionModel model = new DialogueOptionModel() {
                NextDialogId = nextDialogId,
                Message = message,
                Command = command,
                Color = color,
                Requirements = requirements
            };
            return model;
        }

        public override JObject ToJObject(DialogueOptionModel value, JsonSerializer serializer) => new JObject() {
                { nameof(DialogueOptionModel.NextDialogId).ToFirstLower(), value.NextDialogId },
                { nameof(DialogueOptionModel.Message).ToFirstLower(), value.Message },
                { nameof(DialogueOptionModel.Command).ToFirstLower(), value.Command },
                { nameof(DialogueOptionModel.Color).ToFirstLower(), value.Color },
                { nameof(DialogueOptionModel.Requirements).ToFirstLower(), JArray.FromObject(value.Requirements, serializer) },
            };
    }
}
