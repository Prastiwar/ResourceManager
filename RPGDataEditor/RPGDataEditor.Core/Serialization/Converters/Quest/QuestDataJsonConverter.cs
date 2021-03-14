using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace RPGDataEditor.Core.Serialization
{
    public class QuestDataJsonConverter : JsonConverter<QuestModel>
    {
        public override QuestModel ReadJson(JsonReader reader, Type objectType, [AllowNull] QuestModel existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject obj = JObject.Load(reader);
                int id = obj.GetValue(nameof(QuestModel.Id), -1);
                string title = obj.GetValue<string>(nameof(QuestModel.Title));
                string message = obj.GetValue<string>(nameof(QuestModel.Message));
                string category = obj.GetValue<string>(nameof(QuestModel.Category));
                QuestTask completionTask = obj.GetValue<QuestTask>(nameof(QuestModel.CompletionTask));
                IList<QuestTask> tasks = obj.GetValue<IList<QuestTask>>(nameof(QuestModel.Tasks));
                IList<PlayerRequirementModel> requirements = obj.GetValue<IList<PlayerRequirementModel>>(nameof(QuestModel.Requirements));
                QuestModel model = new QuestModel() {
                    Id = id,
                    Title = title,
                    Message = message,
                    Category = category,
                    CompletionTask = completionTask,
                    Tasks = tasks,
                    Requirements = requirements
                };
                return model;
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] QuestModel value, JsonSerializer serializer)
        {
            JObject obj = new JObject() {
                { nameof(QuestModel.Id).ToFirstLower(), value.Id },
                { nameof(QuestModel.Title).ToFirstLower(), value.Title },
                { nameof(QuestModel.Message).ToFirstLower(), value.Message },
                { nameof(QuestModel.Category).ToFirstLower(), value.Category },
                { nameof(QuestModel.CompletionTask).ToFirstLower(), JToken.FromObject(value.CompletionTask, serializer) },
                { nameof(QuestModel.Tasks).ToFirstLower(), JArray.FromObject(value.Tasks, serializer) },
                { nameof(QuestModel.Requirements).ToFirstLower(), JArray.FromObject(value.Requirements, serializer) },
            };
            obj.WriteTo(writer);
        }
    }
}
