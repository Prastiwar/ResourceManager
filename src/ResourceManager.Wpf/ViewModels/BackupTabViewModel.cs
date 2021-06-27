using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using ResourceManager.Data;
using ResourceManager.DataSource;
using ResourceManager.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;

namespace ResourceManager.Wpf.ViewModels
{
    public class BackupTabViewModel : ScreenViewModel
    {
        public BackupTabViewModel(IDataSource dataSource, ITextSerializer textSerializer, IConfiguration configuration, ILogger<BackupTabViewModel> logger)
        {
            DataSource = dataSource;
            TextSerializer = textSerializer;
            Configuration = configuration;
            Logger = logger;
        }

        protected IDataSource DataSource { get; }
        protected ITextSerializer TextSerializer { get; }
        protected IConfiguration Configuration { get; }
        protected ILogger<BackupTabViewModel> Logger { get; }

        private ICommand ceateBackupCommand;
        public ICommand CreateBackupCommand => ceateBackupCommand ??= new DelegateCommand<Type>((type) => CreateBackup(type));

        private string backupFolderPath;
        public string BackupFolderPath {
            get => Configuration["BackupFolderPath"];
            set {
                if (SetProperty(ref backupFolderPath, value))
                {
                    Configuration["BackupFolderPath"] = value;
                }
            }
        }

        private async void CreateBackup(Type resourceType)
        {
            List<object> resources = await DataSource.Query(resourceType).ToListAsync();
            DateTime now = DateTime.Now;
            string fileName = $"{resourceType.Name}-backup-{now.ToString("dd_MM_yyyy")}-{now.ToString("HH_mm_ss")}";
            string filePath = Path.Combine(BackupFolderPath, fileName);
            Type realListType = typeof(List<>).MakeGenericType(resourceType);
            string backup = TextSerializer.Serialize(resources, realListType);
            await File.WriteAllTextAsync(filePath, backup);
        }
    }
}
