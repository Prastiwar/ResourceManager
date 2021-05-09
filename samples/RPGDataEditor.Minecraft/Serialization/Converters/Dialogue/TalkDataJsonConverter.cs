using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ResourceManager;
using RPGDataEditor.Core;
using RPGDataEditor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RPGDataEditor.Minecraft.Serialization
{
    public class TalkDataJsonConverter : Core.Serialization.TalkDataJsonConverter
    {
        public override TalkData ReadJObject(Type objectType, JObject obj)
        {
            TalkData coreModel = base.ReadJObject(objectType, obj);
            IList<TalkLine> interactLines = obj.GetValue<ObservableCollection<TalkLine>>(nameof(Models.TalkData.InteractLines));
            IList<TalkLine> deathLines = obj.GetValue<ObservableCollection<TalkLine>>(nameof(Models.TalkData.DeathLines));
            IList<TalkLine> hurtLines = obj.GetValue<ObservableCollection<TalkLine>>(nameof(Models.TalkData.HurtLines));
            Models.TalkData model = new Models.TalkData() {
                TalkRange = coreModel.TalkRange,
                InteractLines = interactLines,
                DeathLines = deathLines,
                HurtLines = hurtLines
            };
            model.InitationDialogues.AddRange(coreModel.InitationDialogues);
            return model;
        }

        public override JObject ToJObject(TalkData value, JsonSerializer serializer)
        {
            JObject obj = base.ToJObject(value, serializer);
            obj.Add(nameof(Models.TalkData.InteractLines).ToFirstLower(), JArray.FromObject((value as Models.TalkData).InteractLines));
            obj.Add(nameof(Models.TalkData.DeathLines).ToFirstLower(), JArray.FromObject((value as Models.TalkData).DeathLines));
            obj.Add(nameof(Models.TalkData.HurtLines).ToFirstLower(), JArray.FromObject((value as Models.TalkData).HurtLines));
            return obj;
        }
    }
}
