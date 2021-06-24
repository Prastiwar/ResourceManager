using Microsoft.Extensions.Logging;
using Prism.Commands;
using RPGDataEditor.Mvvm;
using System;
using System.Windows.Input;

namespace RPGDataEditor.Wpf.ViewModels
{
    public class SettingsTabViewModel : ScreenViewModel
    {
        public SettingsTabViewModel(ILogger<SettingsTabViewModel> logger) => Logger = logger;

        private ICommand ceateBackupCommand;
        public ICommand CreateBackupCommand => ceateBackupCommand ??= new DelegateCommand<Type>((type) => CreateBackup(type));

        protected ILogger<SettingsTabViewModel> Logger { get; }

        //public override async Task<bool> CanNavigateFrom(INavigationContext navigationContext)
        //{
        // TODO: validate options
        //FluentValidation.Results.ValidationResult validationResult = await Context.Mediator.Send(new ValidateResourceQuery<ISession>(Session));
        //return validationResult.IsValid;
        //}

        //public override async Task OnNavigatedToAsync(INavigationContext navigationContext)
        //{
        //    await base.OnNavigatedToAsync(navigationContext);
        //    if (Session is DefaultSessionContext context)
        //    {
        //        context.Options.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
        //    }
        //}

        //private void NotifyPropertyChanged_PropertyChanged(object sender, PropertyChangedEventArgs e) => Session.SaveSession();

        private async void CreateBackup(Type resourceType)
        {
            // TODO: Create backup
            //bool saved = await Context.Mediator.Send(new BackupResourceQuery(resourceType));
        }
    }
}
