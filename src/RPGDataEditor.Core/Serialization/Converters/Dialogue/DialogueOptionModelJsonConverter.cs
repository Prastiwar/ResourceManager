using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ResourceManager;
using RPGDataEditor.Models;
using System;
using System.Collections.Generic;

namespace RPGDataEditor.Core.Serialization
{
    public class DialogueOptionJsonConverter : ExtendableJsonConverter<DialogueOption>
    {
        public override DialogueOption ReadJObject(Type objectType, JObject obj)
        {
            int nextDialogId = obj.GetValue(nameof(DialogueOption.NextDialogId), -1);
            string message = obj.GetValue<string>(nameof(DialogueOption.Message));
            IList<Requirement> requirements = obj.GetValue<List<Requirement>>(nameof(DialogueOption.Requirements));
            DialogueOption model = new DialogueOption() {
                NextDialogId = nextDialogId,
                Message = message
            };
            model.Requirements.AddRange(requirements);
            return model;
        }

        public override JObject ToJObject(DialogueOption value, JsonSerializer serializer) => new JObject() {
                { nameof(DialogueOption.NextDialogId).ToFirstLower(), JToken.FromObject(value.NextDialogId) },
                { nameof(DialogueOption.Message).ToFirstLower(), value.Message },
                { nameof(DialogueOption.Requirements).ToFirstLower(), JArray.FromObject(value.Requirements, serializer) },
            };
    }
}
