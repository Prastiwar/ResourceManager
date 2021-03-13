using Newtonsoft.Json;
using Prism.Commands;
using Prism.Regions;
using RPGDataEditor.Core;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RPGDataEditor.Wpf.Settings.ViewModels
{
    public class SettingsTabViewModel : TabViewModel
    {
        public SettingsTabViewModel(ViewModelContext context) : base(context) { }

        private ICommand ceateBackupCommand;
        public ICommand CreateBackupCommand => ceateBackupCommand ??= new DelegateCommand<object>((obj) => CreateBackup((RPGResource)obj), obj => obj is RPGResource);

        public override async Task<bool> CanSwitchFrom(NavigationContext navigationContext)
        {
            FluentValidation.Results.ValidationResult validationResult = await Context.ValidationProvider.ValidateAsync(Session);
            return validationResult.IsValid;
        }

        private async void CreateBackup(RPGResource resource)
        {
            FluentValidation.Results.ValidationResult validationResult = await Context.ValidationProvider.ValidateAsync(Session);
            if (!validationResult.IsValid)
            {
                return;
            }
            try
            {
                string relativePath = null;
                string backupPath = null;
                switch (resource)
                {
                    case RPGResource.Quest:
                        relativePath = "quests";
                        backupPath = Session.Options.QuestsBackupPath;
                        break;
                    case RPGResource.Dialogue:
                        relativePath = "dialogues";
                        backupPath = Session.Options.DialoguesBackupPath;
                        break;
                    case RPGResource.Npc:
                        relativePath = "npcs";
                        backupPath = Session.Options.NpcBackupPath;
                        break;
                    default:
                        break;
                }
                if (relativePath != null)
                {
                    if (string.IsNullOrEmpty(backupPath))
                    {
                        Context.SnackbarService.Enqueue("Can't make backup at invalid path");
                        return;
                    }
                    string[] jsons = await Session.GetCurrentController().GetJsonsAsync(relativePath);
                    string backupJson = JsonConvert.SerializeObject(jsons);
                    bool saved = await Session.SaveJsonFileAsync(backupPath, backupJson);
                    Context.SnackbarService.Enqueue(saved ? "Created backup succesfully" : "There was a problem while creating backup");
                }
            }
            catch (System.Exception ex)
            {
                Logger.Error("Couldn't make backup", ex);
                Context.SnackbarService.Enqueue("Couldn't make backup");
            }
        }
    }
}
