using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Minecraft.Serialization
{
    public class TalkDataModelJsonConverter : Core.Serialization.TalkDataModelJsonConverter
    {
        public override TalkDataModel ReadJObject(Type objectType, JObject obj)
        {
            TalkDataModel coreModel = base.ReadJObject(objectType, obj);
            IList<TalkLine> interactLines = obj.GetValue<ObservableCollection<TalkLine>>(nameof(Models.TalkDataModel.InteractLines));
            IList<TalkLine> deathLines = obj.GetValue<ObservableCollection<TalkLine>>(nameof(Models.TalkDataModel.DeathLines));
            IList<TalkLine> hurtLines = obj.GetValue<ObservableCollection<TalkLine>>(nameof(Models.TalkDataModel.HurtLines));
            Models.TalkDataModel model = new Models.TalkDataModel() {
                TalkRange = coreModel.TalkRange,
                InteractLines = interactLines,
                DeathLines = deathLines,
                HurtLines = hurtLines,
                InitationDialogues = coreModel.InitationDialogues
            };
            return model;
        }

        public override JObject ToJObject(TalkDataModel value, JsonSerializer serializer)
        {
            JObject obj = base.ToJObject(value, serializer);
            obj.Add(nameof(Models.TalkDataModel.InteractLines).ToFirstLower(), JArray.FromObject((value as Models.TalkDataModel).InteractLines));
            obj.Add(nameof(Models.TalkDataModel.DeathLines).ToFirstLower(), JArray.FromObject((value as Models.TalkDataModel).DeathLines));
            obj.Add(nameof(Models.TalkDataModel.HurtLines).ToFirstLower(), JArray.FromObject((value as Models.TalkDataModel).HurtLines));
            return obj;
        }
    }
}
