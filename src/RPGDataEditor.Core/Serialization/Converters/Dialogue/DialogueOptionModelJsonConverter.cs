using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ResourceManager;
using RPGDataEditor.Models;
using System;
using System.Collections.Generic;

namespace RPGDataEditor.Core.Serialization
{
    public class DialogueOptionModelJsonConverter : ExtendableJsonConverter<DialogueOptionModel>
    {
        public override DialogueOptionModel ReadJObject(Type objectType, JObject obj)
        {
            int nextDialogId = obj.GetValue(nameof(DialogueOptionModel.NextDialogId), -1);
            string message = obj.GetValue<string>(nameof(DialogueOptionModel.Message));
            IList<PlayerRequirementModel> requirements = obj.GetValue<List<PlayerRequirementModel>>(nameof(DialogueOptionModel.Requirements));
            DialogueOptionModel model = new DialogueOptionModel() {
                NextDialogId = nextDialogId,
                Message = message
            };
            model.Requirements.AddRange(requirements);
            return model;
        }

        public override JObject ToJObject(DialogueOptionModel value, JsonSerializer serializer) => new JObject() {
                { nameof(DialogueOptionModel.NextDialogId).ToFirstLower(), JToken.FromObject(value.NextDialogId) },
                { nameof(DialogueOptionModel.Message).ToFirstLower(), value.Message },
                { nameof(DialogueOptionModel.Requirements).ToFirstLower(), JArray.FromObject(value.Requirements, serializer) },
            };
    }
}
