using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Models;
using System;

namespace RPGDataEditor.Core.Serialization
{
    public class OptionsDataJsonConverter : ExtendableJsonConverter<OptionsData>
    {
        public override OptionsData ReadJObject(Type objectType, JObject obj)
        {
            bool backupOnSaving = obj.GetValue(nameof(OptionsData.BackupOnSaving), false);
            bool prettyPrint = obj.GetValue(nameof(OptionsData.PrettyPrint), true);
            string dialoguesBackupPath = obj.GetValue<string>(nameof(OptionsData.DialoguesBackupPath));
            string npcBackupPath = obj.GetValue<string>(nameof(OptionsData.NpcBackupPath));
            string questsBackupPath = obj.GetValue<string>(nameof(OptionsData.QuestsBackupPath));
            return new OptionsData() {
                BackupOnSaving = backupOnSaving,
                PrettyPrint = prettyPrint,
                DialoguesBackupPath = dialoguesBackupPath,
                NpcBackupPath = npcBackupPath,
                QuestsBackupPath = questsBackupPath
            };
        }

        public override JObject ToJObject(OptionsData value, JsonSerializer serializer) => new JObject() {
                { nameof(value.BackupOnSaving).ToLower(), value.BackupOnSaving },
                { nameof(value.PrettyPrint).ToLower(), value.PrettyPrint },
                { nameof(value.DialoguesBackupPath).ToLower(), value.DialoguesBackupPath },
                { nameof(value.NpcBackupPath).ToLower(), value.NpcBackupPath },
                { nameof(value.QuestsBackupPath).ToLower(), value.QuestsBackupPath },
            };
    }
}
