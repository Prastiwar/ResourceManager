using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                HurtSoundLocation = hurtSoundLocation,
                Attributes = new ObservableCollection<RPGDataEditor.Models.AttributeData>(coreModel.Attributes)
            };
            return model;
        }

        public override JObject ToJObject(RPGDataEditor.Models.Npc value, JsonSerializer serializer)
        {
            JObject obj = base.ToJObject(value, serializer);
            Npc extendedNpc = (value as Npc);
            obj.Add(nameof(Npc.Title).ToFirstLower(), extendedNpc.Title);
            obj.Add(nameof(Npc.TextureLocation).ToFirstLower(), extendedNpc.TextureLocation);
            obj.Add(nameof(Npc.Invulnerable).ToFirstLower(), extendedNpc.Invulnerable);
            obj.Add(nameof(Npc.LookAtPlayer).ToFirstLower(), extendedNpc.LookAtPlayer);
            obj.Add(nameof(Npc.HealthRegen).ToFirstLower(), extendedNpc.HealthRegen);
            obj.Add(nameof(Npc.Equipment).ToFirstLower(), extendedNpc.Equipment != null ? JToken.FromObject(extendedNpc.Equipment, serializer) : null);
            obj.Add(nameof(Npc.MovementType).ToFirstLower(), extendedNpc.MovementType.ToString().ToUpper());
            obj.Add(nameof(Npc.Paths).ToFirstLower(), extendedNpc.Paths != null ? JArray.FromObject(extendedNpc.Paths, serializer) : null);
            obj.Add(nameof(Npc.AmbientSoundLocation).ToFirstLower(), extendedNpc.AmbientSoundLocation);
            obj.Add(nameof(Npc.DeathSoundLocation).ToFirstLower(), extendedNpc.DeathSoundLocation);
            obj.Add(nameof(Npc.HurtSoundLocation).ToFirstLower(), extendedNpc.HurtSoundLocation);
            return obj;
        }
    }
}
