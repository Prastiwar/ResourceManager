using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ResourceManager;
using ResourceManager.DataSource;
using RPGDataEditor.Core.Commands;
using RPGDataEditor.Core.Validation;
using RPGDataEditor.Mvvm;
using RPGDataEditor.Mvvm.Navigation;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace RPGDataEditor.Wpf.Connection.ViewModels
{
    public class ConnectionTabViewModel : ScreenViewModel, IValidationHook
    {
        public ConnectionTabViewModel(ViewModelContext context, IConfigurableDataSource dataSourceConfigurator)
            : base(context) => DataSourceConfigurator = dataSourceConfigurator;

        private IConfiguration configuration;
        public IConfiguration Configuration {
            get => configuration;
            set => SetProperty(ref configuration, value);
        }

        protected IConfigurableDataSource DataSourceConfigurator { get; }

        public event EventHandler<ValidatedEventArgs> Validated;

        protected void RaiseValidated(object instance, ValidationResult result) => Validated?.Invoke(this, new ValidatedEventArgs(instance, result));

        public override Task<bool> CanNavigateTo(INavigationContext navigationContext) => Task.FromResult(true);

        public override async Task<bool> CanNavigateFrom(INavigationContext navigationContext)
        {
            ValidationResult result = await Context.Mediator.Send(new ValidateResourceQuery(typeof(IConfigurationSection), Configuration));
            RaiseValidated(Configuration, result);
            if (result.IsValid)
            {
                DataSourceConfigurator.Configure(Configuration);
                bool connected = await Context.DataSource.Monitor.ForceCheckAsync(default);
                if (!connected)
                {
                    return false;
                }
                try
                {
                    await Context.Persistance.SaveConfigAsync((IConfigurationSection)Configuration);
                }
                catch (Exception ex)
                {
                    Context.Logger.LogError(ex, "Couldn't save session");
                }
            }
            return result.IsValid;
        }

        public override Task OnNavigatedToAsync(INavigationContext navigationContext)
        {
            Configuration = Context.Configuration.GetDataSourceSection();
            Context.DataSource.Monitor.Stop();
            return Task.CompletedTask;
        }

        public override Task OnNavigatedFromAsync(INavigationContext navigationContext)
        {
            DataSourceConfigurator.Configure(Configuration);
            Context.DataSource.Monitor.Start();
            Context.DataSource.Monitor.Changed -= ConnectionMonitor_Changed;
            Context.DataSource.Monitor.Changed += ConnectionMonitor_Changed;
            return Task.CompletedTask;
        }

        private async void ConnectionMonitor_Changed(object sender, bool hasConnection)
        {
            if (!hasConnection)
            {
                await Application.Current.Dispatcher.Invoke(async () => await Context.DialogService.ShowDialogAsync(DialogNames.ConnectionDialog));
            }
        }
    }
}
