using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace RPGDataEditor.Core.Serialization
{
    public class NumberCastsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(int) || objectType == typeof(float) || objectType == typeof(double) || objectType == typeof(decimal);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JValue jsonValue = serializer.Deserialize<JValue>(reader);
            if (jsonValue.Type == JTokenType.Float)
            {
                return Convert.ChangeType(jsonValue.Value<double>(), objectType);
            }
            else if (jsonValue.Type == JTokenType.Integer)
            {
                return Convert.ChangeType(jsonValue.Value<int>(), objectType);
            }
            throw new FormatException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => writer.WriteValue(value);
    }
}
