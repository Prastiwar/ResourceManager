using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace RPGDataEditor.Core.Serialization
{
    public class DialogueOptionModelJsonConverter : JsonConverter<DialogueOptionModel>
    {
        public override DialogueOptionModel ReadJson(JsonReader reader, Type objectType, [AllowNull] DialogueOptionModel existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject obj = JObject.Load(reader);
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
            return null;
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] DialogueOptionModel value, JsonSerializer serializer)
        {
            JObject obj = new JObject() {
                { nameof(DialogueOptionModel.NextDialogId).ToFirstLower(), value.NextDialogId },
                { nameof(DialogueOptionModel.Message).ToFirstLower(), value.Message },
                { nameof(DialogueOptionModel.Command).ToFirstLower(), value.Command },
                { nameof(DialogueOptionModel.Color).ToFirstLower(), value.Color },
                { nameof(DialogueOptionModel.Requirements).ToFirstLower(), JArray.FromObject(value.Requirements, serializer) },
            };
            obj.WriteTo(writer);
        }
    }
}
