using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Models;
using System;

namespace RPGDataEditor.Core.Serialization
{
    public class AttributeDataModelJsonConverter : ExtendableJsonConverter<AttributeDataModel>
    {
        public override AttributeDataModel ReadJObject(Type objectType, JObject obj)
        {
            string name = obj.GetValue<string>(nameof(AttributeDataModel.Name));
            double value = obj.GetValue<double>(nameof(AttributeDataModel.Value), 0.0);
            AttributeDataModel model = new AttributeDataModel() {
                Name = name,
                Value = value
            };
            return model;
        }

        public override JObject ToJObject(AttributeDataModel value, JsonSerializer serializer) => new JObject() {
                { nameof(AttributeDataModel.Name).ToFirstLower(), value.Name },
                { nameof(AttributeDataModel.Value).ToFirstLower(), value.Value }
            };
    }
}
