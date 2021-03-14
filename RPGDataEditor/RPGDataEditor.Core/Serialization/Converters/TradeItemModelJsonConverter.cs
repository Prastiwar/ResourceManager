using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Core.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace RPGDataEditor.Core.Serialization
{
    public class TradeItemModelJsonConverter : JsonConverter<TradeItemModel>
    {
        public override TradeItemModel ReadJson(JsonReader reader, Type objectType, [AllowNull] TradeItemModel existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject obj = JObject.Load(reader);
                string item = obj.GetValue<string>(nameof(TradeItemModel.Item), null);
                int buy = obj.GetValue<int>(nameof(TradeItemModel.Buy), 0);
                int sell = obj.GetValue<int>(nameof(TradeItemModel.Sell), 0);
                int count = obj.GetValue(nameof(TradeItemModel.Count), 1);
                string nbt = obj.GetValue<string>(nameof(TradeItemModel.Nbt), null);
                TradeItemModel model = new TradeItemModel() {
                    Item = item,
                    Buy = buy,
                    Sell = sell,
                    Count = count,
                    Nbt = nbt
                };
                return model;
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] TradeItemModel value, JsonSerializer serializer)
        {
            JObject obj = new JObject() {
                { nameof(TradeItemModel.Item).ToFirstLower(), value.Item },
                { nameof(TradeItemModel.Buy).ToFirstLower(), value.Buy },
                { nameof(TradeItemModel.Sell).ToFirstLower(), value.Sell },
                { nameof(TradeItemModel.Count).ToFirstLower(), value.Count },
                { nameof(TradeItemModel.Nbt).ToFirstLower(), value.Nbt }
            };
            obj.WriteTo(writer);
        }
    }
}
