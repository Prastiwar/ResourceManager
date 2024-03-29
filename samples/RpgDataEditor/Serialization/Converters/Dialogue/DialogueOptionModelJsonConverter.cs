﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ResourceManager;
using ResourceManager.Core;
using ResourceManager.Core.Serialization;
using RpgDataEditor.Models;
using System;
using System.Collections.Generic;

namespace RpgDataEditor.Serialization
{
    public class DialogueOptionJsonConverter : ExtendableJsonConverter<DialogueOption>
    {
        public override DialogueOption ReadJObject(Type objectType, JObject obj)
        {
            int nextDialogId = obj.GetValue<int>(nameof(DialogueOption.NextDialogId), -1);
            string message = obj.GetValue<string>(nameof(DialogueOption.Message));
            IList<Requirement> requirements = obj.GetValue<List<Requirement>>(nameof(DialogueOption.Requirements));
            DialogueOption model = new DialogueOption() {
                NextDialogId = nextDialogId,
                Message = message,
                Requirements = requirements
            };
            return model;
        }

        public override JObject ToJObject(DialogueOption value, JsonSerializer serializer) => new JObject() {
                { nameof(DialogueOption.NextDialogId).ToFirstLower(), value.NextDialogId != null ? JToken.FromObject(value.NextDialogId) : null },
                { nameof(DialogueOption.Message).ToFirstLower(), value.Message },
                { nameof(DialogueOption.Requirements).ToFirstLower(), value.Requirements != null ? JArray.FromObject(value.Requirements, serializer) : null },
            };
    }
}
