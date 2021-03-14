using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Core.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace RPGDataEditor.Core.Serialization
{
    public class EquipmentModelJsonConverter : JsonConverter<EquipmentModel>
    {
        public override EquipmentModel ReadJson(JsonReader reader, Type objectType, [AllowNull] EquipmentModel existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject obj = JObject.Load(reader);
                string head = obj.GetValue<string>(nameof(EquipmentModel.Head), null);
                string chest = obj.GetValue<string>(nameof(EquipmentModel.Head), null);
                string legs = obj.GetValue<string>(nameof(EquipmentModel.Head), null);
                string feet = obj.GetValue<string>(nameof(EquipmentModel.Head), null);
                string mainHand = obj.GetValue<string>(nameof(EquipmentModel.Head), null);
                string offHand = obj.GetValue<string>(nameof(EquipmentModel.Head), null);
                EquipmentModel model = new EquipmentModel() {
                    Head = head,
                    Chest = chest,
                    Legs = legs,
                    Feet = feet,
                    MainHand = mainHand,
                    OffHand = offHand
                };
                return model;
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] EquipmentModel value, JsonSerializer serializer)
        {
            JObject obj = new JObject() {
                { nameof(EquipmentModel.Head).ToFirstLower(), value.Head },
                { nameof(EquipmentModel.Chest).ToFirstLower(), value.Chest },
                { nameof(EquipmentModel.Legs).ToFirstLower(), value.Legs },
                { nameof(EquipmentModel.Feet).ToFirstLower(), value.Feet },
                { nameof(EquipmentModel.MainHand).ToFirstLower(), value.MainHand },
                { nameof(EquipmentModel.OffHand).ToFirstLower(), value.OffHand }
            };
            obj.WriteTo(writer);
        }
    }
}
