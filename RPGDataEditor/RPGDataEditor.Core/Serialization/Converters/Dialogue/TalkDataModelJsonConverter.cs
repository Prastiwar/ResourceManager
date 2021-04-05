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
            IList<int> initationDialogues = obj.GetValue<ObservableCollection<int>>(nameof(TalkDataModel.InitationDialogues));
            TalkDataModel model = new TalkDataModel() {
                TalkRange = talkRange,
                InitationDialogues = initationDialogues
            };
            return model;
        }

        public override JObject ToJObject(TalkDataModel value, JsonSerializer serializer) => new JObject() {
                { nameof(TalkDataModel.TalkRange).ToFirstLower(), value.TalkRange },
                { nameof(TalkDataModel.InitationDialogues).ToFirstLower(), JArray.FromObject(value.InitationDialogues) },
            };
    }
}
