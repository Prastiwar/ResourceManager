using AutoUpdaterDotNET;
using DryIoc;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Newtonsoft.Json;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using ResourceManager;
using ResourceManager.Data;
using ResourceManager.Services;
using RPGDataEditor.Connection;
using RPGDataEditor.Core;
using RPGDataEditor.Core.Commands;
using RPGDataEditor.Core.Connection;
using RPGDataEditor.Core.Serialization;
using RPGDataEditor.Core.Services;
using RPGDataEditor.Extensions.Prism.Wpf.Services;
using RPGDataEditor.Mvvm;
using RPGDataEditor.Mvvm.Services;
using RPGDataEditor.Providers;
using RPGDataEditor.Services;
using RPGDataEditor.Wpf.Connection;
using RPGDataEditor.Wpf.Mvvm;
using RPGDataEditor.Wpf.Providers;
using RPGDataEditor.Wpf.Services;
using RPGDataEditor.Wpf.Views;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Windows;

namespace RPGDataEditor.Wpf
{
    public class RpgDataEditorApplication : PrismApplication
    {
        public RpgDataEditorApplication()
        {
            DispatcherUnhandledException += OnUnhandledException;
            Exit += OnExit;
        }

        public static new RpgDataEditorApplication Current => Application.Current as RpgDataEditorApplication;

        protected string AppRootPath => new DirectoryInfo(BinaryPath).Parent.FullName;

        protected string BinaryPath => Path.Combine(Environment.CurrentDirectory + "bin");

        protected virtual string CacheDirectoryPath {
            get {
                string path = Path.Combine(AppRootPath, "cache");
                Directory.CreateDirectory(path);
                return path;
            }
        }

        protected virtual string SessionFilePath => Path.Combine(CacheDirectoryPath, "session.json");

        protected virtual void OnUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
            => Container.Resolve<ILogger>().Error("Unhandled exception", e.Exception);

        protected virtual void OnExit(object sender, ExitEventArgs e) { }

        protected override Window CreateShell() => Container.Resolve<MainWindow>();

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
            regionAdapterMappings.RegisterMapping(typeof(System.Windows.Controls.TabControl), Container.Resolve<TabControlAdapter>());
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog) => moduleCatalog.AddModule<TabModule>();

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            JsonConvert.DefaultSettings = CreateJsonSettings;
            RegisterLogging(containerRegistry);
            try
            {
                RegisterConfiguration(containerRegistry);
            }
            catch (Exception ex)
            {
                containerRegistry.GetContainer().Resolve<ILogger>().Error("Couldn't load configuration", ex);
                throw;
            }

            RegisterAssemblyScanner(containerRegistry);
            RegisterValidators(containerRegistry);
            RegisterProviders(containerRegistry);
            RegisterServices(containerRegistry);
            RegisterDialogs(containerRegistry);
            RegisterDescriptors(containerRegistry);
            RegisterMediator(containerRegistry);

            InitializeAutoUpdater();

            containerRegistry.RegisterSingleton<ViewModelContext>();

            OnRegistrationFinished(containerRegistry);
        }

        protected virtual JsonSerializerSettings CreateJsonSettings()
        {
            PrettyOrderPropertyResolver propResolver = new PrettyOrderPropertyResolver();
            propResolver.SetAllLetterCase(Lettercase.CamelCase);
            JsonSerializerSettings settings = new JsonSerializerSettings {
                ContractResolver = propResolver,
                Formatting = Formatting.Indented
            };
            settings.Converters.Add(new NumberCastsConverter());
            settings.Converters.Add(new PlayerRequirementJsonConverter());

            settings.Converters.Add(new NpcJobJsonConverter());
            settings.Converters.Add(new NpcJsonConverter());
            settings.Converters.Add(new TradeItemJsonConverter());
            settings.Converters.Add(new AttributeDataModelJsonConverter());

            settings.Converters.Add(new PositionJsonConverter());

            settings.Converters.Add(new QuestTaskJsonConverter());
            settings.Converters.Add(new QuestJsonConverter());

            settings.Converters.Add(new DialogueJsonConverter());
            settings.Converters.Add(new DialogueOptionJsonConverter());
            settings.Converters.Add(new TalkDataJsonConverter());
            settings.Converters.Add(new TalkLineJsonConverter());

            settings.Converters.Add(new ConnectionSettingsJsonConverter());
            return settings;
        }

        protected virtual void RegisterLogging(IContainerRegistry containerRegistry) => containerRegistry.RegisterInstance<ILogger>(new LocalFileLogger());

        protected virtual void RegisterConfiguration(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<RPGDataEditor.Services.ISerializer, NewtonsoftSerializer>();
            containerRegistry.RegisterSingleton<IAppPersistanceService, LocalAppPersistanceService>();
            containerRegistry.RegisterSingleton<IConnectionConfiguration, ConnectionConfiguration>();

            containerRegistry.RegisterInstance<IFileClient>(new LocalFileClient() { FileSearchPattern = "*.json" });
            containerRegistry.RegisterInstance<IFtpFileClient>(new FtpFileClient());
            containerRegistry.RegisterInstance<ISqlClient>(new SqlClient());
        }

        protected virtual void RegisterAssemblyScanner(IContainerRegistry containerRegistry)
        {
            AssemblyName[] referencedAssemblies = Assembly.GetEntryAssembly().GetReferencedAssemblies();
            Assembly[] assembliesToScan = new Assembly[referencedAssemblies.Length + 1];
            assembliesToScan[0] = Assembly.GetEntryAssembly();
            for (int i = 1; i < assembliesToScan.Length; i++)
            {
                assembliesToScan[i] = Assembly.Load(referencedAssemblies[i - 1]);
            }
            containerRegistry.RegisterInstance<IFluentAssemblyScanner>(new FluentAssemblyScanner(assembliesToScan));
        }

        protected virtual void RegisterValidators(IContainerRegistry containerRegistry)
        {
            IFluentAssemblyScanner scanner = containerRegistry.GetContainer().Resolve<IFluentAssemblyScanner>();
            TypeScanResult results = scanner.Scan().ScanTypes(t => !t.IsGenericType).Select(typeof(AbstractValidator<>)).Get();
            foreach (Type validatorType in results.Scans.First().ResultTypes)
            {
                Type interfaceType = validatorType.GetInterfaces().First(i => typeof(IValidator<>).IsAssignableFrom(i.GetGenericTypeDefinition()));
                containerRegistry.Register(interfaceType, validatorType);
            }
        }

        protected virtual void RegisterProviders(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton(typeof(IImplementationProvider<>), typeof(DefaultImplementationProvider<>));
            AutoTemplateProvider controlProvider = new AutoTemplateProvider(Container);
            controlProvider.RegisterDefaults(containerRegistry);
            containerRegistry.RegisterInstance<IAutoTemplateProvider>(controlProvider);
            containerRegistry.RegisterSingleton<IServiceProvider, PrismServiceProvider>();
        }

        protected virtual void RegisterServices(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ISnackbarService, SnackbarService>();
            containerRegistry.RegisterSingleton<IDialogService, PrismDialogService>();
        }

        protected virtual void RegisterDialogs(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<UpdateDialog>(typeof(UpdateDialog).Name);
            containerRegistry.RegisterDialog<ConnectionDialog>(typeof(ConnectionDialog).Name);
        }

        protected virtual void RegisterDescriptors(IContainerRegistry containerRegistry)
        {
            ResourceDescriptorService service = new ResourceDescriptorService();
            IResourceDescriptor fileQuestDescriptor = new FileQuestPathResourceDescriptor();
            IResourceDescriptor sqlQuestDescriptor = new SqlQuestPathResourceDescriptor();
            service.Register<Models.Quest>(fileQuestDescriptor, sqlQuestDescriptor);

            IResourceDescriptor fileDialogueDescriptor = new FileDialoguePathResourceDescriptor();
            IResourceDescriptor sqlDialogueDescriptor = new SqlDialoguePathResourceDescriptor();
            service.Register<Models.Dialogue>(fileDialogueDescriptor, sqlDialogueDescriptor);

            IResourceDescriptor fileNpcDescriptor = new FileNpcPathResourceDescriptor();
            IResourceDescriptor sqlNpcDescriptor = new SqlNpcPathResourceDescriptor();
            service.Register<Models.Npc>(fileNpcDescriptor, sqlNpcDescriptor);

            containerRegistry.RegisterInstance<IResourceDescriptorService>(service);
        }

        protected virtual void InitializeAutoUpdater()
        {
            AutoUpdater.AppCastURL = "https://raw.githubusercontent.com/Prastiwar/RPGDataEditor/main/version.json";
            AutoUpdater.ParseUpdateInfoEvent += AutoUpdater_ParseUpdateInfoEvent;
        }

        private void AutoUpdater_ParseUpdateInfoEvent(ParseUpdateInfoEventArgs args)
        {
            UpdateInfo updateInfo = JsonConvert.DeserializeObject<UpdateInfo>(args.RemoteData);
            args.UpdateInfo = new UpdateInfoEventArgs() {
                CurrentVersion = updateInfo.Version,
                DownloadURL = updateInfo.Url,
                ChangelogURL = updateInfo.Changelog,
                InstallerArgs = updateInfo.Args,
                Mandatory = updateInfo.Mandatory,
                CheckSum = updateInfo.CheckSum
            };
        }

        protected virtual void RegisterMediator(IContainerRegistry containerRegistry)
        {
            IFluentAssemblyScanner scanner = containerRegistry.GetContainer().Resolve<IFluentAssemblyScanner>();

            Type genericNotificationHandlerType = typeof(INotificationHandler<>);
            TypeScan scan = scanner.Scan().Select(genericNotificationHandlerType).Get().Scans.First();
            foreach (Type result in scan.ResultTypes)
            {
                System.Collections.Generic.IEnumerable<Type> handlerInterfaces = result.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericNotificationHandlerType);
                foreach (Type handlerInterface in handlerInterfaces)
                {
                    containerRegistry.RegisterSingleton(handlerInterface, result);
                }
            }

            Type genericHandlerType = typeof(IRequestHandler<,>);
            scan = scanner.Scan().Select(genericHandlerType).Get().Scans.First();
            foreach (Type result in scan.ResultTypes)
            {
                System.Collections.Generic.IEnumerable<Type> handlerInterfaces = result.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericHandlerType);
                foreach (Type handlerInterface in handlerInterfaces)
                {
                    containerRegistry.RegisterSingleton(handlerInterface, result);
                }
            }

            Type genericPipelineType = typeof(IPipelineBehavior<,>);
            scan = scanner.Scan().Select(genericPipelineType).Get().Scans.First();
            foreach (Type result in scan.ResultTypes)
            {
                if (result.IsGenericType)
                {
                    continue;
                }
                System.Collections.Generic.IEnumerable<Type> pipelineInterfaces = result.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericPipelineType);
                foreach (Type pipelineInterface in pipelineInterfaces)
                {
                    containerRegistry.RegisterSingleton(pipelineInterface, result);
                }
            }

            containerRegistry.RegisterSingleton(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            containerRegistry.RegisterSingleton(typeof(IRequestHandler<ValidateResourceQuery, ValidationResult>), typeof(ValidateResourceHandler));
            containerRegistry.RegisterInstance(typeof(ServiceFactory), (ServiceFactory)Container.Resolve);
            containerRegistry.RegisterSingleton<IMediator, Mediator>();
        }

        protected virtual void OnRegistrationFinished(IContainerRegistry containerRegistry)
        {
            FileInfo sessionFile = new FileInfo(SessionFilePath);
            IConnectionSettings settings = null;
            if (sessionFile.Exists)
            {
                string json = File.ReadAllText(SessionFilePath);
                settings = JsonConvert.DeserializeObject<IConnectionSettings>(json);
            }
            IConnectionConfig config = settings != null ? settings.CreateConfig() : new ConnectionSettings() { Type = ConnectionSettings.Connection.LOCAL }.CreateConfig();
            IConnectionConfiguration configuration = containerRegistry.GetContainer().Resolve<IConnectionConfiguration>();
            configuration.Configure(config);

            if (containerRegistry.GetContainer().Resolve<IAppPersistanceService>() is LocalAppPersistanceService service)
            {
                service.FolderPath = SessionFilePath;
            }

            configuration.Configured += Configuration_Configured;
        }

        private void Configuration_Configured(object sender, IConnectionConfig e)
        {
            string type = (string)e.Get(nameof(ConnectionSettings.Type));
            if (type == ConnectionSettings.Connection.LOCAL && Container.Resolve<IFileClient>() is LocalFileClient localClient)
            {
                localClient.FolderPath = e.Get(nameof(LocalFileClient.FolderPath), "").ToString();
            }
            if (type == ConnectionSettings.Connection.FTP && Container.Resolve<IFtpFileClient>() is FtpFileClient ftpClient)
            {
                ftpClient.Host = e.Get(nameof(FtpFileClient.Host), "").ToString();
                ftpClient.RelativePath = e.Get(nameof(FtpFileClient.RelativePath), "").ToString();
                ftpClient.UserName = e.Get(nameof(FtpFileClient.UserName), "").ToString();
                ftpClient.Password = e.Get(nameof(FtpFileClient.Password), new SecureString()) as SecureString;
                ftpClient.Port = (int)e.Get(nameof(FtpFileClient.Port), 0);
            }
            if (type == ConnectionSettings.Connection.SQL && Container.Resolve<ISqlClient>() is SqlClient sqlClient)
            {
                sqlClient.ConnectionString = e.Get(nameof(SqlClient.ConnectionString), "").ToString();
            }
            // TODO: configure registration of command handlers
        }
    }
}
