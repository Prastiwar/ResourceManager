using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ResourceManager;
using ResourceManager.Core;
using ResourceManager.Core.Serialization;
using RpgDataEditor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RpgDataEditor.Serialization
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
                { nameof(Npc.Position).ToFirstLower(), value.Position != null ? JToken.FromObject(value.Position, serializer) : null },
                { nameof(Npc.Attributes).ToFirstLower(), value.Attributes != null ? JArray.FromObject(value.Attributes, serializer) : null },
                { nameof(Npc.Job).ToFirstLower(), value.Job != null ? JToken.FromObject(value.Job, serializer) : null },
                { nameof(Npc.TalkData).ToFirstLower(), value.TalkData != null ? JToken.FromObject(value.TalkData, serializer) : null }
            };
    }
}
