using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RPGDataEditor.Core.Serialization
{
    public class NpcJsonConverter : ExtendableJsonConverter<Npc>
    {
        public override Npc ReadJObject(Type objectType, JObject obj)
        {
            object id = obj.GetValue<object>(nameof(Npc.Id));
            string name = obj.GetValue<string>(nameof(Npc.Name));
            Position position = obj.GetValue<Position>(nameof(Npc.Position));
            IList<AttributeData> attributes = obj.GetValue<ObservableCollection<AttributeData>>(nameof(Npc.Attributes));
            NpcJob job = obj.GetValue<NpcJob>(nameof(Npc.Job));
            TalkData talkData = obj.GetValue<TalkData>(nameof(Npc.TalkData));
            Npc model = new Npc() {
                Id = id,
                Name = name,
                Position = position,
                Job = job,
                TalkData = talkData,
                Attributes = attributes
            };
            return model;
        }

        public override JObject ToJObject(Npc value, JsonSerializer serializer) => new JObject() {
                { nameof(Npc.Id).ToFirstLower(), JToken.FromObject(value.Id) },
                { nameof(Npc.Name).ToFirstLower(), value.Name },
                { nameof(Npc.Position).ToFirstLower(), JToken.FromObject(value.Position, serializer) },
                { nameof(Npc.Attributes).ToFirstLower(), JArray.FromObject(value.Attributes, serializer) },
                { nameof(Npc.Job).ToFirstLower(), JToken.FromObject(value.Job, serializer) },
                { nameof(Npc.TalkData).ToFirstLower(), JToken.FromObject(value.TalkData, serializer) }
            };
    }
}
