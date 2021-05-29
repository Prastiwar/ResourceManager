using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Core;
using RPGDataEditor.Models;
using System;
using System.Collections.ObjectModel;

namespace RPGDataEditor.Minecraft.Serialization
{
    public class DialogueJsonConverter : Core.Serialization.DialogueJsonConverter
    {
        public override Dialogue ReadJObject(Type objectType, JObject obj)
        {
            Dialogue coreModel = base.ReadJObject(objectType, obj);
            bool allowEscape = obj.GetValue(nameof(Models.Dialogue.AllowEscape), false);
            Models.Dialogue model = new Models.Dialogue() {
                Id = coreModel.Id,
                Title = coreModel.Title,
                Message = coreModel.Message,
                Category = coreModel.Category,
                AllowEscape = allowEscape,
                StartQuestId = coreModel.StartQuestId,
                Requirements = new ObservableCollection<Requirement>(coreModel.Requirements),
                Options = new ObservableCollection<DialogueOption>(coreModel.Options),
            };
            return model;
        }

        public override JObject ToJObject(Dialogue value, JsonSerializer serializer)
        {
            JObject obj = base.ToJObject(value, serializer);
            obj.Add(nameof(Models.Dialogue.AllowEscape).ToFirstLower(), (value as Models.Dialogue).AllowEscape);
            return obj;
        }
    }
}
