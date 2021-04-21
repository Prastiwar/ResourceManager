using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ResourceManager;
using RPGDataEditor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RPGDataEditor.Core.Serialization
{
    public class QuestDataJsonConverter : ExtendableJsonConverter<QuestModel>
    {
        public override QuestModel ReadJObject(Type objectType, JObject obj)
        {
            object id = obj.GetValue<object>(nameof(QuestModel.Id));
            string title = obj.GetValue<string>(nameof(QuestModel.Title));
            string message = obj.GetValue<string>(nameof(QuestModel.Message));
            string category = obj.GetValue<string>(nameof(QuestModel.Category));
            QuestTask completionTask = obj.GetValue<QuestTask>(nameof(QuestModel.CompletionTask));
            IList<QuestTask> tasks = obj.GetValue<ObservableCollection<QuestTask>>(nameof(QuestModel.Tasks));
            IList<PlayerRequirementModel> requirements = obj.GetValue<ObservableCollection<PlayerRequirementModel>>(nameof(QuestModel.Requirements));
            QuestModel model = new QuestModel() {
                Id = id,
                Title = title,
                Message = message,
                Category = category,
                CompletionTask = completionTask
            };
            model.Requirements.AddRange(requirements);
            model.Tasks.AddRange(tasks);
            return model;
        }

        public override JObject ToJObject(QuestModel value, JsonSerializer serializer) => new JObject() {
                { nameof(QuestModel.Id).ToFirstLower(), JToken.FromObject(value.Id) },
                { nameof(QuestModel.Title).ToFirstLower(), value.Title },
                { nameof(QuestModel.Message).ToFirstLower(), value.Message },
                { nameof(QuestModel.Category).ToFirstLower(), value.Category },
                { nameof(QuestModel.CompletionTask).ToFirstLower(), JToken.FromObject(value.CompletionTask, serializer) },
                { nameof(QuestModel.Tasks).ToFirstLower(), JArray.FromObject(value.Tasks, serializer) },
                { nameof(QuestModel.Requirements).ToFirstLower(), JArray.FromObject(value.Requirements, serializer) },
            };
    }
}
