using Prism.Commands;
using Prism.Regions;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;
using System.ComponentModel;
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

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext)
        {
            await base.OnNavigatedToAsync(navigationContext);
            if (Session is DefaultSessionContext context)
            {
                context.Options.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
            }
        }

        private void NotifyPropertyChanged_PropertyChanged(object sender, PropertyChangedEventArgs e) => Session.SaveSession();

        private async void CreateBackup(RPGResource resource)
        {
            FluentValidation.Results.ValidationResult validationResult = await Context.ValidationProvider.ValidateAsync(Session);
            if (!validationResult.IsValid)
            {
                return;
            }
            bool saved = await Session.CreateBackupAsync(resource);
            Context.SnackbarService.Enqueue(saved ? "Created backup succesfully" : "There was a problem while creating backup");
        }
    }
}
