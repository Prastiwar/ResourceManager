﻿using Microsoft.Extensions.Configuration;
using RPGDataEditor.Services;
using System.Threading.Tasks;

namespace RPGDataEditor
{
    public static class ConfigurationExtensions
    {
        public const string SessionFileName = "session";

        public static Task SaveConfigAsync(this IAppPersistanceService service, IConfigurationSection section) => service.SaveAsync(SessionFileName, section);

        public static void SaveConfig(this IAppPersistanceService service, IConfigurationSection section) => service.Save(SessionFileName, section);
    }
}