using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RPGDataEditor.Core.Serialization
{
    public class NpcDataModelJsonConverter : ExtendableJsonConverter<NpcDataModel>
    {
        public override NpcDataModel ReadJObject(Type objectType, JObject obj)
        {
            int id = obj.GetValue(nameof(NpcDataModel.Id), -1);
            string name = obj.GetValue<string>(nameof(NpcDataModel.Name));
            Position position = obj.GetValue<Position>(nameof(NpcDataModel.Position));
            IList<AttributeDataModel> attributes = obj.GetValue<ObservableCollection<AttributeDataModel>>(nameof(NpcDataModel.Attributes));
            NpcJobModel job = obj.GetValue<NpcJobModel>(nameof(NpcDataModel.Job));
            TalkDataModel talkData = obj.GetValue<TalkDataModel>(nameof(NpcDataModel.TalkData));
            NpcDataModel model = new NpcDataModel() {
                Id = id,
                Name = name,
                Position = position,
                Attributes = attributes,
                Job = job,
                TalkData = talkData
            };
            return model;
        }

        public override JObject ToJObject(NpcDataModel value, JsonSerializer serializer) => new JObject() {
                { nameof(NpcDataModel.Id).ToFirstLower(), value.Id },
                { nameof(NpcDataModel.Name).ToFirstLower(), value.Name },
                { nameof(NpcDataModel.Position).ToFirstLower(), JToken.FromObject(value.Position, serializer) },
                { nameof(NpcDataModel.Attributes).ToFirstLower(), JArray.FromObject(value.Attributes, serializer) },
                { nameof(NpcDataModel.Job).ToFirstLower(), JToken.FromObject(value.Job, serializer) },
                { nameof(NpcDataModel.TalkData).ToFirstLower(), JToken.FromObject(value.TalkData, serializer) }
            };
    }
}
