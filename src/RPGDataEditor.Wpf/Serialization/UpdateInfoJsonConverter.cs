using AutoUpdaterDotNET;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Core;
using RPGDataEditor.Core.Serialization;
using System;

namespace RPGDataEditor.Wpf.Serialization
{
    public class UpdateInfoJsonConverter : ExtendableJsonConverter<UpdateInfo>
    {
        public override UpdateInfo ReadJObject(Type objectType, JObject obj)
        {
            string version = obj.GetValue<string>(nameof(UpdateInfo.Version));
            string url = obj.GetValue<string>(nameof(UpdateInfo.Url));
            string changelog = obj.GetValue<string>(nameof(UpdateInfo.Changelog));
            string args = obj.GetValue<string>(nameof(UpdateInfo.Args));
            Mandatory mandatory = obj.GetValue<Mandatory>(nameof(UpdateInfo.Mandatory));
            CheckSum checkSum = obj.GetValue<CheckSum>(nameof(UpdateInfo.CheckSum));
            return new UpdateInfo() {
                Version = version,
                Url = url,
                Changelog = changelog,
                Args = args,
                Mandatory = mandatory,
                CheckSum = checkSum
            };
        }

        public override JObject ToJObject(UpdateInfo value, JsonSerializer serializer) => new JObject() {
            { nameof(UpdateInfo.Version), value.Version },
            { nameof(UpdateInfo.Url), value.Url },
            { nameof(UpdateInfo.Changelog), value.Changelog },
            { nameof(UpdateInfo.Args), value.Args },
            { nameof(UpdateInfo.Mandatory), JToken.FromObject(value.Mandatory) },
            { nameof(UpdateInfo.CheckSum), JToken.FromObject(value.CheckSum) }
        };
    }
}
