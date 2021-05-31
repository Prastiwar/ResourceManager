using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ResourceManager;
using ResourceManager.DataSource;
using RPGDataEditor.Core.Commands;
using RPGDataEditor.Core.Validation;
using RPGDataEditor.Mvvm;
using RPGDataEditor.Mvvm.Commands;
using RPGDataEditor.Mvvm.Navigation;
using RPGDataEditor.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Windows;

namespace RPGDataEditor.Wpf.Connection.ViewModels
{
    public class ObservableDictionary<TValue> : IDictionary<string, TValue>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        private Dictionary<string, TValue> internalDictionary = new Dictionary<string, TValue>();

        public TValue this[string key] {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public ICollection<string> Keys => throw new NotImplementedException();

        public ICollection<TValue> Values => throw new NotImplementedException();

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public event PropertyChangedEventHandler PropertyChanged;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void Add(string key, TValue value) => internalDictionary.Add(key, value);
        public void Add(KeyValuePair<string, TValue> item) => internalDictionary.Add(item);
        public void Clear() => internalDictionary.Clear();
        public bool Contains(KeyValuePair<string, TValue> item) => internal.Contains(item);
        public bool ContainsKey(string key) => throw new NotImplementedException();
        public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex) => throw new NotImplementedException();
        public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator() => throw new NotImplementedException();
        public bool Remove(string key) => throw new NotImplementedException();
        public bool Remove(KeyValuePair<string, TValue> item) => throw new NotImplementedException();
        public bool TryGetValue(string key, [MaybeNullWhen(false)] out TValue value) => throw new NotImplementedException();
        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
    }

    public class ConnectionTabViewModel : ScreenViewModel, IValidationHook
    {
        public ConnectionTabViewModel(IMediator mediator, IConfigurableDataSource configurator, IAppPersistanceService persistance, IConfiguration configuration, ILogger<ConnectionTabViewModel> logger)
        {
            Mediator = mediator;
            Configurator = configurator;
            Persistance = persistance;
            ConfigurationRoot = configuration;
            Logger = logger;
        }

        private IConfiguration configuration;
        public IConfiguration Configuration {
            get => configuration;
            set => SetProperty(ref configuration, value);
        }

        public ObservableDictionary

        protected IMediator Mediator { get; }

        protected IConfigurableDataSource Configurator { get; }

        protected IAppPersistanceService Persistance { get; }

        protected IConfiguration ConfigurationRoot { get; }

        protected ILogger<ConnectionTabViewModel> Logger { get; }

        public event EventHandler<ValidatedEventArgs> Validated;

        protected void RaiseValidated(object instance, ValidationResult result) => Validated?.Invoke(this, new ValidatedEventArgs(instance, result));

        public override Task<bool> CanNavigateTo(INavigationContext navigationContext) => Task.FromResult(true);

        public override async Task<bool> CanNavigateFrom(INavigationContext navigationContext)
        {
            ValidationResult result = await Mediator.Send(new ValidateResourceQuery(typeof(IConfigurationSection), Configuration));
            RaiseValidated(Configuration, result);
            if (result.IsValid)
            {
                Configurator.Configure(Configuration);
                bool connected = await Configurator.CurrentSource.Monitor.ForceCheckAsync(default);
                if (!connected)
                {
                    return false;
                }
                try
                {
                    await Persistance.SaveConfigAsync((IConfigurationSection)Configuration);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Couldn't save session");
                }
            }
            return result.IsValid;
        }

        public override Task OnNavigatedToAsync(INavigationContext navigationContext)
        {
            Configuration = ConfigurationRoot.GetDataSourceSection();
            Configurator.CurrentSource.Monitor.Stop();
            return Task.CompletedTask;
        }

        public override Task OnNavigatedFromAsync(INavigationContext navigationContext)
        {
            Configurator.Configure(Configuration);
            Configurator.CurrentSource.Monitor.Start();
            Configurator.CurrentSource.Monitor.Changed -= ConnectionMonitor_Changed;
            Configurator.CurrentSource.Monitor.Changed += ConnectionMonitor_Changed;
            return Task.CompletedTask;
        }

        private async void ConnectionMonitor_Changed(object sender, bool hasConnection)
        {
            if (!hasConnection)
            {
                await Application.Current.Dispatcher.Invoke(async () => await Mediator.Send(new ShowDialogQuery(DialogNames.ConnectionDialog, null)));
            }
        }
    }
}
