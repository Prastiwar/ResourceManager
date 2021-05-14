using MediatR;
using Microsoft.Extensions.Configuration;
using ResourceManager.DataSource;
using RPGDataEditor.Mvvm.Services;
using RPGDataEditor.Services;

namespace RPGDataEditor.Mvvm
{
    public class ViewModelContext
    {
        public ViewModelContext(IMediator mediator,
                                IAppPersistanceService persistance,
                                IConfiguration configuration,
                                IDataSource dataSource,
                                IDialogService dialogService,
                                ILogger logger)
        {
            Mediator = mediator;
            Persistance = persistance;
            Configuration = configuration;
            DataSource = dataSource;
            DialogService = dialogService;
            Logger = logger;
        }

        public IMediator Mediator { get; }
        public IAppPersistanceService Persistance { get; }
        public IConfiguration Configuration { get; }
        public IDataSource DataSource { get; }
        public IDialogService DialogService { get; }
        public ILogger Logger { get; }
    }
}
