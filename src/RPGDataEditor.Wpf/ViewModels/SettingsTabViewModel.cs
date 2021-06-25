using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using ResourceManager.DataSource;
using RPGDataEditor.Mvvm;
using RPGDataEditor.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;

namespace RPGDataEditor.Wpf.ViewModels
{
    public class SettingsTabViewModel : ScreenViewModel
    {
        public SettingsTabViewModel(IDataSource dataSource, IConfiguration configuration, IAppPersistanceService persistanceService, ILogger<SettingsTabViewModel> logger)
        {
            DataSource = dataSource;
            Configuration = configuration;
            PersistanceService = persistanceService;
            Logger = logger;
        }

        protected IDataSource DataSource { get; }
        protected IConfiguration Configuration { get; }
        protected IAppPersistanceService PersistanceService { get; }
        protected ILogger<SettingsTabViewModel> Logger { get; }

        private ICommand ceateBackupCommand;
        public ICommand CreateBackupCommand => ceateBackupCommand ??= new DelegateCommand<Type>((type) => CreateBackup(type));

        private string backupFolderPath;
        public string BackupFolderPath {
            get => Configuration["BackupFolderPath"];
            set {
                if (SetProperty(ref backupFolderPath, value))
                {
                    Configuration["BackupFolderPath"] = value;
                    PersistanceService.SaveConfig(Configuration);
                }
            }
        }

        private async void CreateBackup(Type resourceType)
        {
            List<object> resources = await DataSource.Query(resourceType).ToListAsync();
            DateTime now = DateTime.Now;
            string fileName = $"{resourceType.Name}-backup-{now.ToString("dd_MM_yyyy")}-{now.ToString("HH_mm_ss")}";
            string filePath = Path.Combine(BackupFolderPath, fileName);
            string backup = ""; // TODO: Create backup
            await File.WriteAllTextAsync(filePath, backup);
        }
    }
}
