using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace RPGDataEditor.Core.Serialization
{
    public class TalkDataModelJsonConverter : JsonConverter<TalkDataModel>
    {
        public override TalkDataModel ReadJson(JsonReader reader, Type objectType, [AllowNull] TalkDataModel existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject obj = JObject.Load(reader);
                int talkRange = obj.GetValue<int>(nameof(TalkDataModel.TalkRange), 0);
                IList<TalkLine> interactLines = obj.GetValue<IList<TalkLine>>(nameof(TalkDataModel.InteractLines));
                IList<TalkLine> deathLines = obj.GetValue<IList<TalkLine>>(nameof(TalkDataModel.DeathLines));
                IList<TalkLine> hurtLines = obj.GetValue<IList<TalkLine>>(nameof(TalkDataModel.HurtLines));
                IList<int> initiationDialogues = obj.GetValue<IList<int>>(nameof(TalkDataModel.InitationDialogues));
                TalkDataModel model = new TalkDataModel() {
                    TalkRange = talkRange,
                    InteractLines = interactLines,
                    DeathLines = deathLines,
                    HurtLines = hurtLines,
                    InitationDialogues = initiationDialogues
                };
                return model;
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] TalkDataModel value, JsonSerializer serializer)
        {
            JObject obj = new JObject() {
                { nameof(TalkDataModel.TalkRange).ToFirstLower(), value.TalkRange },
                { nameof(TalkDataModel.InteractLines).ToFirstLower(), JArray.FromObject(value.InteractLines) },
                { nameof(TalkDataModel.DeathLines).ToFirstLower(), JArray.FromObject(value.DeathLines) },
                { nameof(TalkDataModel.HurtLines).ToFirstLower(), JArray.FromObject(value.HurtLines) },
                { nameof(TalkDataModel.InitationDialogues).ToFirstLower(), JArray.FromObject(value.InitationDialogues) },
            };
            obj.WriteTo(writer);
        }
    }
}
