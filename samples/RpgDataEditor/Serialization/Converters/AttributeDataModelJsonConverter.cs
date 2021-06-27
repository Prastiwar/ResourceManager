using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ResourceManager;
using ResourceManager.Core;
using ResourceManager.Core.Serialization;
using RpgDataEditor.Models;
using System;

namespace RpgDataEditor.Serialization
{
    public class AttributeDataModelJsonConverter : ExtendableJsonConverter<AttributeData>
    {
        public override AttributeData ReadJObject(Type objectType, JObject obj)
        {
            string name = obj.GetValue<string>(nameof(AttributeData.Name));
            double value = obj.GetValue<double>(nameof(AttributeData.Value), 0.0);
            AttributeData model = new AttributeData() {
                Name = name,
                Value = value
            };
            return model;
        }

        public override JObject ToJObject(AttributeData value, JsonSerializer serializer) => new JObject() {
                { nameof(AttributeData.Name).ToFirstLower(), value.Name },
                { nameof(AttributeData.Value).ToFirstLower(), value.Value }
            };
    }
}
