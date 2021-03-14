using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace RPGDataEditor.Core.Serialization
{
    public class DialogueModelJsonConverter : JsonConverter<DialogueModel>
    {
        public override DialogueModel ReadJson(JsonReader reader, Type objectType, [AllowNull] DialogueModel existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject obj = JObject.Load(reader);
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
            return null;
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] DialogueModel value, JsonSerializer serializer)
        {
            JObject obj = new JObject() {
                { nameof(DialogueModel.Id).ToFirstLower(), value.Id },
                { nameof(DialogueModel.Title).ToFirstLower(), value.Title },
                { nameof(DialogueModel.Message).ToFirstLower(), value.Message },
                { nameof(DialogueModel.Category).ToFirstLower(), value.Category },
                { nameof(DialogueModel.AllowEscape).ToFirstLower(), value.AllowEscape },
                { nameof(DialogueModel.StartQuest).ToFirstLower(), value.StartQuest },
                { nameof(DialogueModel.Requirements).ToFirstLower(), JArray.FromObject(value.Requirements, serializer) },
                { nameof(DialogueModel.Options).ToFirstLower(), JToken.FromObject(value.Options, serializer) },
            };
            obj.WriteTo(writer);
        }
    }
}
