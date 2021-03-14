using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace RPGDataEditor.Core.Serialization
{
    public class NpcDataModelJsonConverter : JsonConverter<NpcDataModel>
    {
        public override NpcDataModel ReadJson(JsonReader reader, Type objectType, [AllowNull] NpcDataModel existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject obj = JObject.Load(reader);
                int id = obj.GetValue(nameof(NpcDataModel.Id), -1);
                string name = obj.GetValue<string>(nameof(NpcDataModel.Name));
                string title = obj.GetValue<string>(nameof(NpcDataModel.Title));
                string textureLocation = obj.GetValue<string>(nameof(NpcDataModel.TextureLocation));
                Position position = obj.GetValue<Position>(nameof(NpcDataModel.Position));
                bool invulnerable = obj.GetValue(nameof(NpcDataModel.Invulnerable), true);
                bool lookAtPlayer = obj.GetValue(nameof(NpcDataModel.LookAtPlayer), false);
                int healthRegen = obj.GetValue(nameof(NpcDataModel.HealthRegen), 1);
                IList<AttributeDataModel> attributes = obj.GetValue<ObservableCollection<AttributeDataModel>>(nameof(NpcDataModel.Attributes));
                EquipmentModel equipment = obj.GetValue<EquipmentModel>(nameof(NpcDataModel.Equipment));
                NpcJobModel job = obj.GetValue<NpcJobModel>(nameof(NpcDataModel.Job));
                NpcMovement movementType = Enum.Parse<NpcMovement>(obj.GetValue(nameof(NpcDataModel.MovementType), NpcMovement.STATIC.ToString()));
                IList<Position> paths = obj.GetValue<ObservableCollection<Position>>(nameof(NpcDataModel.Paths));
                TalkDataModel talkData = obj.GetValue<TalkDataModel>(nameof(NpcDataModel.TalkData));
                string ambientSoundLocation = obj.GetValue<string>(nameof(NpcDataModel.AmbientSoundLocation));
                string deathSoundLocation = obj.GetValue<string>(nameof(NpcDataModel.DeathSoundLocation));
                string hurtSoundLocation = obj.GetValue<string>(nameof(NpcDataModel.HurtSoundLocation));
                NpcDataModel model = new NpcDataModel() {
                    Id = id,
                    Name = name,
                    Title = title,
                    TextureLocation = textureLocation,
                    Position = position,
                    Invulnerable = invulnerable,
                    LookAtPlayer = lookAtPlayer,
                    HealthRegen = healthRegen,
                    Attributes = attributes,
                    Equipment = equipment,
                    Job = job,
                    MovementType = movementType,
                    Paths = paths,
                    TalkData = talkData,
                    AmbientSoundLocation = ambientSoundLocation,
                    DeathSoundLocation = deathSoundLocation,
                    HurtSoundLocation = hurtSoundLocation
                };
                return model;
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] NpcDataModel value, JsonSerializer serializer)
        {
            JObject obj = new JObject() {
                { nameof(NpcDataModel.Id).ToFirstLower(), value.Id },
                { nameof(NpcDataModel.Name).ToFirstLower(), value.Name },
                { nameof(NpcDataModel.Title).ToFirstLower(), value.Title },
                { nameof(NpcDataModel.TextureLocation).ToFirstLower(), value.TextureLocation },
                { nameof(NpcDataModel.Position).ToFirstLower(), JToken.FromObject(value.Position, serializer) },
                { nameof(NpcDataModel.Invulnerable).ToFirstLower(), value.Invulnerable },
                { nameof(NpcDataModel.LookAtPlayer).ToFirstLower(), value.LookAtPlayer },
                { nameof(NpcDataModel.HealthRegen).ToFirstLower(), value.HealthRegen },
                { nameof(NpcDataModel.Attributes).ToFirstLower(), JArray.FromObject(value.Attributes, serializer) },
                { nameof(NpcDataModel.Equipment).ToFirstLower(), JToken.FromObject(value.Equipment, serializer) },
                { nameof(NpcDataModel.Job).ToFirstLower(), JToken.FromObject(value.Job, serializer) },
                { nameof(NpcDataModel.MovementType).ToFirstLower(), value.MovementType.ToString().ToUpper() },
                { nameof(NpcDataModel.Paths).ToFirstLower(), JArray.FromObject(value.Paths, serializer) },
                { nameof(NpcDataModel.TalkData).ToFirstLower(), JToken.FromObject(value.TalkData, serializer) },
                { nameof(NpcDataModel.AmbientSoundLocation).ToFirstLower(), value.AmbientSoundLocation },
                { nameof(NpcDataModel.DeathSoundLocation).ToFirstLower(), value.DeathSoundLocation },
                { nameof(NpcDataModel.HurtSoundLocation).ToFirstLower(), value.HurtSoundLocation }
            };
            obj.WriteTo(writer);
        }
    }
}
