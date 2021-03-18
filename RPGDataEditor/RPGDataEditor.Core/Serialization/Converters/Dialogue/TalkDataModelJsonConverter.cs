using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RPGDataEditor.Core.Serialization
{
    public class TalkDataModelJsonConverter : ExtendableJsonConverter<TalkDataModel>
    {
        public override TalkDataModel ReadJObject(Type objectType, JObject obj)
        {
            int talkRange = obj.GetValue<int>(nameof(TalkDataModel.TalkRange), 0);
            IList<TalkLine> interactLines = obj.GetValue<ObservableCollection<TalkLine>>(nameof(TalkDataModel.InteractLines));
            IList<TalkLine> deathLines = obj.GetValue<ObservableCollection<TalkLine>>(nameof(TalkDataModel.DeathLines));
            IList<TalkLine> hurtLines = obj.GetValue<ObservableCollection<TalkLine>>(nameof(TalkDataModel.HurtLines));
            IList<int> initationDialogues = obj.GetValue<ObservableCollection<int>>(nameof(TalkDataModel.InitationDialogues));
            TalkDataModel model = new TalkDataModel() {
                TalkRange = talkRange,
                InteractLines = interactLines,
                DeathLines = deathLines,
                HurtLines = hurtLines,
                InitationDialogues = initationDialogues
            };
            return model;
        }

        public override JObject ToJObject(TalkDataModel value, JsonSerializer serializer) => new JObject() {
                { nameof(TalkDataModel.TalkRange).ToFirstLower(), value.TalkRange },
                { nameof(TalkDataModel.InteractLines).ToFirstLower(), JArray.FromObject(value.InteractLines) },
                { nameof(TalkDataModel.DeathLines).ToFirstLower(), JArray.FromObject(value.DeathLines) },
                { nameof(TalkDataModel.HurtLines).ToFirstLower(), JArray.FromObject(value.HurtLines) },
                { nameof(TalkDataModel.InitationDialogues).ToFirstLower(), JArray.FromObject(value.InitationDialogues) },
            };
    }
}
