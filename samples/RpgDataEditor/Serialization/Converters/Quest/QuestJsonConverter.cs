using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ResourceManager;
using ResourceManager.Core;
using ResourceManager.Core.Serialization;
using RpgDataEditor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RpgDataEditor.Serialization
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
                CompletionTask = completionTask,
                Tasks = tasks,
                Requirements = requirements
            };
            return model;
        }

        public override JObject ToJObject(Quest value, JsonSerializer serializer) => new JObject() {
                { nameof(Quest.Id).ToFirstLower(), JToken.FromObject(value.Id) },
                { nameof(Quest.Title).ToFirstLower(), value.Title },
                { nameof(Quest.Message).ToFirstLower(), value.Message },
                { nameof(Quest.Category).ToFirstLower(), value.Category },
                { nameof(Quest.CompletionTask).ToFirstLower(), value.CompletionTask != null ? JToken.FromObject(value.CompletionTask, serializer) : null },
                { nameof(Quest.Tasks).ToFirstLower(), value.Tasks != null ? JArray.FromObject(value.Tasks, serializer) : null },
                { nameof(Quest.Requirements).ToFirstLower(), value.Requirements != null ? JArray.FromObject(value.Requirements, serializer) : null },
            };
    }
}
