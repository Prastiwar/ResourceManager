using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RPGDataEditor.Core.Serialization
{
    public class DialogueJsonConverter : ExtendableJsonConverter<Dialogue>
    {
        public override Dialogue ReadJObject(Type objectType, JObject obj)
        {
            object id = obj.GetValue<object>(nameof(Dialogue.Id));
            string title = obj.GetValue<string>(nameof(Dialogue.Title));
            string message = obj.GetValue<string>(nameof(Dialogue.Message));
            string category = obj.GetValue<string>(nameof(Dialogue.Category));
            int startQuest = obj.GetValue(nameof(Dialogue.StartQuestId), -1);
            IList<Requirement> requirements = obj.GetValue<List<Requirement>>(nameof(Dialogue.Requirements));
            IList<DialogueOption> options = obj.GetValue<List<DialogueOption>>(nameof(Dialogue.Options));
            Dialogue model = new Dialogue() {
                Id = id,
                Title = title,
                Message = message,
                Category = category,
                StartQuestId = startQuest,
                Requirements = new ObservableCollection<Requirement>(requirements),
                Options = new ObservableCollection<DialogueOption>(options),
            };
            return model;
        }

        public override JObject ToJObject(Dialogue value, JsonSerializer serializer) => new JObject() {
                { nameof(Dialogue.Id).ToFirstLower(), JToken.FromObject(value.Id) },
                { nameof(Dialogue.Title).ToFirstLower(), value.Title },
                { nameof(Dialogue.Message).ToFirstLower(), value.Message },
                { nameof(Dialogue.Category).ToFirstLower(), value.Category },
                { nameof(Dialogue.StartQuestId).ToFirstLower(), value.StartQuestId != null ? JToken.FromObject(value.StartQuestId) : null },
                { nameof(Dialogue.Requirements).ToFirstLower(), value.Requirements != null ? JArray.FromObject(value.Requirements, serializer) : null },
                { nameof(Dialogue.Options).ToFirstLower(), value.Options != null ? JToken.FromObject(value.Options, serializer) : null },
            };
    }
}
