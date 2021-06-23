using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                HurtLines = hurtLines,
                InitationDialogues = new ObservableCollection<int>(coreModel.InitationDialogues)
            };
            return model;
        }

        public override JObject ToJObject(TalkData value, JsonSerializer serializer)
        {
            JObject obj = base.ToJObject(value, serializer);
            Models.TalkData extendedData = (value as Models.TalkData);
            obj.Add(nameof(Models.TalkData.InteractLines).ToFirstLower(), extendedData.InteractLines != null ? JArray.FromObject(extendedData.InteractLines) : null);
            obj.Add(nameof(Models.TalkData.DeathLines).ToFirstLower(), extendedData.DeathLines != null ? JArray.FromObject(extendedData.DeathLines) : null);
            obj.Add(nameof(Models.TalkData.HurtLines).ToFirstLower(), extendedData.HurtLines != null ? JArray.FromObject(extendedData.HurtLines) : null);
            return obj;
        }
    }
}
