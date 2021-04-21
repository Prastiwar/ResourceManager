using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ResourceManager;
using RPGDataEditor.Models;
using System;
using System.Collections.Generic;

namespace RPGDataEditor.Core.Serialization
{
    public class NpcDataModelJsonConverter : ExtendableJsonConverter<NpcDataModel>
    {
        public override NpcDataModel ReadJObject(Type objectType, JObject obj)
        {
            object id = obj.GetValue<object>(nameof(NpcDataModel.Id));
            string name = obj.GetValue<string>(nameof(NpcDataModel.Name));
            Position position = obj.GetValue<Position>(nameof(NpcDataModel.Position), default);
            IList<AttributeDataModel> attributes = obj.GetValue<List<AttributeDataModel>>(nameof(NpcDataModel.Attributes));
            NpcJobModel job = obj.GetValue<NpcJobModel>(nameof(NpcDataModel.Job));
            TalkDataModel talkData = obj.GetValue<TalkDataModel>(nameof(NpcDataModel.TalkData));
            NpcDataModel model = new NpcDataModel() {
                Id = id,
                Name = name,
                Position = position,
                Job = job,
                TalkData = talkData
            };
            model.Attributes.AddRange(attributes);
            return model;
        }

        public override JObject ToJObject(NpcDataModel value, JsonSerializer serializer) => new JObject() {
                { nameof(NpcDataModel.Id).ToFirstLower(), JToken.FromObject(value.Id) },
                { nameof(NpcDataModel.Name).ToFirstLower(), value.Name },
                { nameof(NpcDataModel.Position).ToFirstLower(), JToken.FromObject(value.Position, serializer) },
                { nameof(NpcDataModel.Attributes).ToFirstLower(), JArray.FromObject(value.Attributes, serializer) },
                { nameof(NpcDataModel.Job).ToFirstLower(), JToken.FromObject(value.Job, serializer) },
                { nameof(NpcDataModel.TalkData).ToFirstLower(), JToken.FromObject(value.TalkData, serializer) }
            };
    }
}
