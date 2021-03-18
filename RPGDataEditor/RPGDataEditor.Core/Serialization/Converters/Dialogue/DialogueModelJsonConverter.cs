using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RPGDataEditor.Core.Serialization
{
    public class DialogueModelJsonConverter : ExtendableJsonConverter<DialogueModel>
    {
        public override DialogueModel ReadJObject(Type objectType, JObject obj)
        {
            int id = obj.GetValue(nameof(DialogueModel.Id), -1);
            string title = obj.GetValue<string>(nameof(DialogueModel.Title));
            string message = obj.GetValue<string>(nameof(DialogueModel.Message));
            string category = obj.GetValue<string>(nameof(DialogueModel.Category));
            bool allowEscape = obj.GetValue(nameof(DialogueModel.AllowEscape), false);
            int startQuest = obj.GetValue(nameof(DialogueModel.StartQuest), -1);
            IList<PlayerRequirementModel> requirements = obj.GetValue<ObservableCollection<PlayerRequirementModel>>(nameof(DialogueModel.Requirements));
            IList<DialogueOptionModel> options = obj.GetValue<ObservableCollection<DialogueOptionModel>>(nameof(DialogueModel.Options));
            DialogueModel model = new DialogueModel() {
                Id = id,
                Title = title,
                Message = message,
                Category = category,
                AllowEscape = allowEscape,
                StartQuest = startQuest,
                Requirements = requirements,
                Options = options
            };
            return model;
        }

        public override JObject ToJObject(DialogueModel value, JsonSerializer serializer) => new JObject() {
                { nameof(DialogueModel.Id).ToFirstLower(), value.Id },
                { nameof(DialogueModel.Title).ToFirstLower(), value.Title },
                { nameof(DialogueModel.Message).ToFirstLower(), value.Message },
                { nameof(DialogueModel.Category).ToFirstLower(), value.Category },
                { nameof(DialogueModel.AllowEscape).ToFirstLower(), value.AllowEscape },
                { nameof(DialogueModel.StartQuest).ToFirstLower(), value.StartQuest },
                { nameof(DialogueModel.Requirements).ToFirstLower(), JArray.FromObject(value.Requirements, serializer) },
                { nameof(DialogueModel.Options).ToFirstLower(), JToken.FromObject(value.Options, serializer) },
            };
    }
}
