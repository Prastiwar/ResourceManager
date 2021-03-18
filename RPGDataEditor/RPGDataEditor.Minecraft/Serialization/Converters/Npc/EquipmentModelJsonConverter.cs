using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Core;
using RPGDataEditor.Core.Serialization;
using RPGDataEditor.Minecraft.Models;
using System;

namespace RPGDataEditor.Minecraft.Serialization
{
    public class EquipmentModelJsonConverter : ExtendableJsonConverter<EquipmentModel>
    {
        public override EquipmentModel ReadJObject(Type objectType, JObject obj)
        {
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

        public override JObject ToJObject(EquipmentModel value, JsonSerializer serializer) => new JObject() {
                { nameof(EquipmentModel.Head).ToFirstLower(), value.Head },
                { nameof(EquipmentModel.Chest).ToFirstLower(), value.Chest },
                { nameof(EquipmentModel.Legs).ToFirstLower(), value.Legs },
                { nameof(EquipmentModel.Feet).ToFirstLower(), value.Feet },
                { nameof(EquipmentModel.MainHand).ToFirstLower(), value.MainHand },
                { nameof(EquipmentModel.OffHand).ToFirstLower(), value.OffHand }
            };
    }
}
