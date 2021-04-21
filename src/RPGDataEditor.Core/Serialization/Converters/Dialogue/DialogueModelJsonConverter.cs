using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ResourceManager;
using RPGDataEditor.Models;
using System;
using System.Collections.Generic;

namespace RPGDataEditor.Core.Serialization
{
    public class DialogueModelJsonConverter : ExtendableJsonConverter<DialogueModel>
    {
        public override DialogueModel ReadJObject(Type objectType, JObject obj)
        {
            object id = obj.GetValue<object>(nameof(DialogueModel.Id));
            string title = obj.GetValue<string>(nameof(DialogueModel.Title));
            string message = obj.GetValue<string>(nameof(DialogueModel.Message));
            string category = obj.GetValue<string>(nameof(DialogueModel.Category));
            int startQuest = obj.GetValue(nameof(DialogueModel.StartQuestId), -1);
            IList<PlayerRequirementModel> requirements = obj.GetValue<List<PlayerRequirementModel>>(nameof(DialogueModel.Requirements));
            IList<DialogueOptionModel> options = obj.GetValue<List<DialogueOptionModel>>(nameof(DialogueModel.Options));
            DialogueModel model = new DialogueModel() {
                Id = id,
                Title = title,
                Message = message,
                Category = category,
                StartQuestId = startQuest
            };
            model.Requirements.AddRange(requirements);
            model.Options.AddRange(options);
            return model;
        }

        public override JObject ToJObject(DialogueModel value, JsonSerializer serializer) => new JObject() {
                { nameof(DialogueModel.Id).ToFirstLower(), JToken.FromObject(value.Id) },
                { nameof(DialogueModel.Title).ToFirstLower(), value.Title },
                { nameof(DialogueModel.Message).ToFirstLower(), value.Message },
                { nameof(DialogueModel.Category).ToFirstLower(), value.Category },
                { nameof(DialogueModel.StartQuestId).ToFirstLower(), JToken.FromObject(value.StartQuestId) },
                { nameof(DialogueModel.Requirements).ToFirstLower(), JArray.FromObject(value.Requirements, serializer) },
                { nameof(DialogueModel.Options).ToFirstLower(), JToken.FromObject(value.Options, serializer) },
            };
    }
}
