using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ResourceManager;
using RPGDataEditor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RPGDataEditor.Core.Serialization
{
    public class QuestJsonConverter : ExtendableJsonConverter<Quest>
    {
        public override Quest ReadJObject(Type objectType, JObject obj)
        {
            object id = obj.GetValue<object>(nameof(Quest.Id));
            string title = obj.GetValue<string>(nameof(Quest.Title));
            string message = obj.GetValue<string>(nameof(Quest.Message));
            string category = obj.GetValue<string>(nameof(Quest.Category));
            IQuestTask completionTask = obj.GetValue<IQuestTask>(nameof(Quest.CompletionTask));
            IList<IQuestTask> tasks = obj.GetValue<ObservableCollection<IQuestTask>>(nameof(Quest.Tasks));
            IList<Requirement> requirements = obj.GetValue<ObservableCollection<Requirement>>(nameof(Quest.Requirements));
            Quest model = new Quest() {
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

        public override JObject ToJObject(Quest value, JsonSerializer serializer) => new JObject() {
                { nameof(Quest.Id).ToFirstLower(), JToken.FromObject(value.Id) },
                { nameof(Quest.Title).ToFirstLower(), value.Title },
                { nameof(Quest.Message).ToFirstLower(), value.Message },
                { nameof(Quest.Category).ToFirstLower(), value.Category },
                { nameof(Quest.CompletionTask).ToFirstLower(), JToken.FromObject(value.CompletionTask, serializer) },
                { nameof(Quest.Tasks).ToFirstLower(), JArray.FromObject(value.Tasks, serializer) },
                { nameof(Quest.Requirements).ToFirstLower(), JArray.FromObject(value.Requirements, serializer) },
            };
    }
}
