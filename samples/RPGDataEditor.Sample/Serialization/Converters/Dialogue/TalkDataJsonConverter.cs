﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Core;
using RPGDataEditor.Core.Serialization;
using RPGDataEditor.Sample.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RPGDataEditor.Sample.Serialization
{
    public class TalkDataJsonConverter : ExtendableJsonConverter<TalkData>
    {
        public override TalkData ReadJObject(Type objectType, JObject obj)
        {
            int talkRange = obj.GetValue<int>(nameof(TalkData.TalkRange), 0);
            IList<int> initationDialogues = obj.GetValue<ObservableCollection<int>>(nameof(TalkData.InitationDialogues));
            TalkData model = new TalkData() {
                TalkRange = talkRange,
                InitationDialogues = initationDialogues
            };
            return model;
        }

        public override JObject ToJObject(TalkData value, JsonSerializer serializer) => new JObject() {
                { nameof(TalkData.TalkRange).ToFirstLower(), value.TalkRange },
                { nameof(TalkData.InitationDialogues).ToFirstLower(), value.InitationDialogues != null ? JArray.FromObject(value.InitationDialogues) : null },
            };
    }
}
