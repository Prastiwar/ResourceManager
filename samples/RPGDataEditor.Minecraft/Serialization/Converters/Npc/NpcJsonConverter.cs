using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ResourceManager;
using RPGDataEditor.Core;
using RPGDataEditor.Minecraft.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RPGDataEditor.Minecraft.Serialization
{
    public class NpcJsonConverter : Core.Serialization.NpcJsonConverter
    {
        public override RPGDataEditor.Models.Npc ReadJObject(Type objectType, JObject obj)
        {
            RPGDataEditor.Models.Npc coreModel = base.ReadJObject(objectType, obj);
            string title = obj.GetValue<string>(nameof(Npc.Title));
            string textureLocation = obj.GetValue<string>(nameof(Npc.TextureLocation));
            bool invulnerable = obj.GetValue(nameof(Npc.Invulnerable), true);
            bool lookAtPlayer = obj.GetValue(nameof(Npc.LookAtPlayer), false);
            int healthRegen = obj.GetValue(nameof(Npc.HealthRegen), 1);
            Equipment equipment = obj.GetValue<Equipment>(nameof(Npc.Equipment));
            NpcMovement movementType = Enum.Parse<NpcMovement>(obj.GetValue(nameof(Npc.MovementType), NpcMovement.STATIC.ToString()));
            IList<RPGDataEditor.Models.Position> paths = obj.GetValue<ObservableCollection<RPGDataEditor.Models.Position>>(nameof(Npc.Paths));
            string ambientSoundLocation = obj.GetValue<string>(nameof(Npc.AmbientSoundLocation));
            string deathSoundLocation = obj.GetValue<string>(nameof(Npc.DeathSoundLocation));
            string hurtSoundLocation = obj.GetValue<string>(nameof(Npc.HurtSoundLocation));
            Npc model = new Npc() {
                Id = coreModel.Id,
                Name = coreModel.Name,
                Title = title,
                TextureLocation = textureLocation,
                Position = coreModel.Position,
                Invulnerable = invulnerable,
                LookAtPlayer = lookAtPlayer,
                HealthRegen = healthRegen,
                Equipment = equipment,
                Job = coreModel.Job,
                MovementType = movementType,
                Paths = paths,
                TalkData = coreModel.TalkData,
                AmbientSoundLocation = ambientSoundLocation,
                DeathSoundLocation = deathSoundLocation,
                HurtSoundLocation = hurtSoundLocation
            };
            model.Attributes.AddRange(coreModel.Attributes);
            return model;
        }

        public override JObject ToJObject(RPGDataEditor.Models.Npc value, JsonSerializer serializer)
        {
            JObject obj = base.ToJObject(value, serializer);
            obj.Add(nameof(Npc.Title).ToFirstLower(), (value as Npc).Title);
            obj.Add(nameof(Npc.TextureLocation).ToFirstLower(), (value as Npc).TextureLocation);
            obj.Add(nameof(Npc.Invulnerable).ToFirstLower(), (value as Npc).Invulnerable);
            obj.Add(nameof(Npc.LookAtPlayer).ToFirstLower(), (value as Npc).LookAtPlayer);
            obj.Add(nameof(Npc.HealthRegen).ToFirstLower(), (value as Npc).HealthRegen);
            obj.Add(nameof(Npc.Equipment).ToFirstLower(), JToken.FromObject((value as Npc).Equipment, serializer));
            obj.Add(nameof(Npc.MovementType).ToFirstLower(), (value as Npc).MovementType.ToString().ToUpper());
            obj.Add(nameof(Npc.Paths).ToFirstLower(), JArray.FromObject((value as Npc).Paths, serializer));
            obj.Add(nameof(Npc.AmbientSoundLocation).ToFirstLower(), (value as Npc).AmbientSoundLocation);
            obj.Add(nameof(Npc.DeathSoundLocation).ToFirstLower(), (value as Npc).DeathSoundLocation);
            obj.Add(nameof(Npc.HurtSoundLocation).ToFirstLower(), (value as Npc).HurtSoundLocation);
            return obj;
        }
    }
}
