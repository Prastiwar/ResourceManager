﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ResourceManager;
using ResourceManager.Core;
using ResourceManager.Core.Serialization;
using RpgDataEditor.Models;
using System;

namespace RpgDataEditor.Serialization
{
    public class TalkLineJsonConverter : ExtendableJsonConverter<TalkLine>
    {
        public override TalkLine ReadJObject(Type objectType, JObject obj)
        {
            string text = obj.GetValue<string>(nameof(TalkLine.Text));
            object soundId = obj.GetValue<object>(nameof(TalkLine.SoundId));
            TalkLine model = new TalkLine() {
                Text = text,
                SoundId = soundId
            };
            return model;
        }

        public override JObject ToJObject(TalkLine value, JsonSerializer serializer) => new JObject() {
                { nameof(TalkLine.Text).ToFirstLower(), value.Text },
                { nameof(TalkLine.SoundId).ToFirstLower(), value.SoundId != null ? JToken.FromObject(value.SoundId) : null }
            };
    }
}
