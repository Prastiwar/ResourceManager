using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Core;
using RPGDataEditor.Minecraft.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RPGDataEditor.Minecraft.Serialization
{
    public class NpcDataModelJsonConverter : Core.Serialization.NpcDataModelJsonConverter
    {
        public override Core.Models.NpcDataModel ReadJObject(Type objectType, JObject obj)
        {
            Core.Models.NpcDataModel coreModel = base.ReadJObject(objectType, obj);
            string title = obj.GetValue<string>(nameof(NpcDataModel.Title));
            string textureLocation = obj.GetValue<string>(nameof(NpcDataModel.TextureLocation));
            bool invulnerable = obj.GetValue(nameof(NpcDataModel.Invulnerable), true);
            bool lookAtPlayer = obj.GetValue(nameof(NpcDataModel.LookAtPlayer), false);
            int healthRegen = obj.GetValue(nameof(NpcDataModel.HealthRegen), 1);
            EquipmentModel equipment = obj.GetValue<EquipmentModel>(nameof(NpcDataModel.Equipment));
            NpcMovement movementType = Enum.Parse<NpcMovement>(obj.GetValue(nameof(NpcDataModel.MovementType), NpcMovement.STATIC.ToString()));
            IList<Core.Models.Position> paths = obj.GetValue<ObservableCollection<Core.Models.Position>>(nameof(NpcDataModel.Paths));
            string ambientSoundLocation = obj.GetValue<string>(nameof(NpcDataModel.AmbientSoundLocation));
            string deathSoundLocation = obj.GetValue<string>(nameof(NpcDataModel.DeathSoundLocation));
            string hurtSoundLocation = obj.GetValue<string>(nameof(NpcDataModel.HurtSoundLocation));
            NpcDataModel model = new NpcDataModel() {
                Id = coreModel.Id,
                Name = coreModel.Name,
                Title = title,
                TextureLocation = textureLocation,
                Position = coreModel.Position,
                Invulnerable = invulnerable,
                LookAtPlayer = lookAtPlayer,
                HealthRegen = healthRegen,
                Attributes = coreModel.Attributes,
                Equipment = equipment,
                Job = coreModel.Job,
                MovementType = movementType,
                Paths = paths,
                TalkData = coreModel.TalkData,
                AmbientSoundLocation = ambientSoundLocation,
                DeathSoundLocation = deathSoundLocation,
                HurtSoundLocation = hurtSoundLocation
            };
            return model;
        }

        public override JObject ToJObject(Core.Models.NpcDataModel value, JsonSerializer serializer)
        {
            JObject obj = base.ToJObject(value, serializer);
            obj.Add(nameof(NpcDataModel.Title).ToFirstLower(), (value as NpcDataModel).Title);
            obj.Add(nameof(NpcDataModel.TextureLocation).ToFirstLower(), (value as NpcDataModel).TextureLocation);
            obj.Add(nameof(NpcDataModel.Invulnerable).ToFirstLower(), (value as NpcDataModel).Invulnerable);
            obj.Add(nameof(NpcDataModel.LookAtPlayer).ToFirstLower(), (value as NpcDataModel).LookAtPlayer);
            obj.Add(nameof(NpcDataModel.HealthRegen).ToFirstLower(), (value as NpcDataModel).HealthRegen);
            obj.Add(nameof(NpcDataModel.Equipment).ToFirstLower(), JToken.FromObject((value as NpcDataModel).Equipment, serializer));
            obj.Add(nameof(NpcDataModel.MovementType).ToFirstLower(), (value as NpcDataModel).MovementType.ToString().ToUpper());
            obj.Add(nameof(NpcDataModel.Paths).ToFirstLower(), JArray.FromObject((value as NpcDataModel).Paths, serializer));
            obj.Add(nameof(NpcDataModel.AmbientSoundLocation).ToFirstLower(), (value as NpcDataModel).AmbientSoundLocation);
            obj.Add(nameof(NpcDataModel.DeathSoundLocation).ToFirstLower(), (value as NpcDataModel).DeathSoundLocation);
            obj.Add(nameof(NpcDataModel.HurtSoundLocation).ToFirstLower(), (value as NpcDataModel).HurtSoundLocation);
            return obj;
        }
    }
}
