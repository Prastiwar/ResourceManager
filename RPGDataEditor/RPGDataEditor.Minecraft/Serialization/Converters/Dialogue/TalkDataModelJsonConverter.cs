using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Minecraft.Models;
using RPGDataEditor.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RPGDataEditor.Minecraft.Serialization
{
    public class TalkDataModelJsonConverter : Core.Serialization.TalkDataModelJsonConverter
    {
        public override Core.Models.TalkDataModel ReadJObject(Type objectType, JObject obj)
        {
            Core.Models.TalkDataModel coreModel = base.ReadJObject(objectType, obj);
            IList<TalkLine> interactLines = obj.GetValue<ObservableCollection<TalkLine>>(nameof(TalkDataModel.InteractLines));
            IList<TalkLine> deathLines = obj.GetValue<ObservableCollection<TalkLine>>(nameof(TalkDataModel.DeathLines));
            IList<TalkLine> hurtLines = obj.GetValue<ObservableCollection<TalkLine>>(nameof(TalkDataModel.HurtLines));
            TalkDataModel model = new TalkDataModel() {
                TalkRange = coreModel.TalkRange,
                InteractLines = interactLines,
                DeathLines = deathLines,
                HurtLines = hurtLines,
                InitationDialogues = coreModel.InitationDialogues
            };
            return model;
        }

        public override JObject ToJObject(Core.Models.TalkDataModel value, JsonSerializer serializer)
        {
            JObject obj = base.ToJObject(value, serializer);
            obj.Add(nameof(TalkDataModel.InteractLines).ToFirstLower(), JArray.FromObject(value.InteractLines));
            obj.Add(nameof(TalkDataModel.DeathLines).ToFirstLower(), JArray.FromObject(value.DeathLines));
            obj.Add(nameof(TalkDataModel.HurtLines).ToFirstLower(), JArray.FromObject(value.HurtLines));
            return obj;
        }
    }
}
