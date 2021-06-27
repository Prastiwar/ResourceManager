using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Core;
using RPGDataEditor.Minecraft.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace RPGDataEditor.Minecraft.Serialization
{
    public class EquipmentJsonConverter : JsonConverter<Equipment>
    {
        public override Equipment ReadJson(JsonReader reader, Type objectType, [AllowNull] Equipment existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject obj = JObject.Load(reader);
                string head = obj.GetValue<string>(nameof(Equipment.Head), null);
                string chest = obj.GetValue<string>(nameof(Equipment.Chest), null);
                string legs = obj.GetValue<string>(nameof(Equipment.Legs), null);
                string feet = obj.GetValue<string>(nameof(Equipment.Feet), null);
                string mainHand = obj.GetValue<string>(nameof(Equipment.MainHand), null);
                string offHand = obj.GetValue<string>(nameof(Equipment.OffHand), null);
                Equipment model = new Equipment() {
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

        public override void WriteJson(JsonWriter writer, [AllowNull] Equipment value, JsonSerializer serializer)
        {
            JObject obj = new JObject() {
                { nameof(Equipment.Head).ToFirstLower(), value.Head },
                { nameof(Equipment.Chest).ToFirstLower(), value.Chest },
                { nameof(Equipment.Legs).ToFirstLower(), value.Legs },
                { nameof(Equipment.Feet).ToFirstLower(), value.Feet },
                { nameof(Equipment.MainHand).ToFirstLower(), value.MainHand },
                { nameof(Equipment.OffHand).ToFirstLower(), value.OffHand }
            };
            obj.WriteTo(writer);
        }
    }
}
