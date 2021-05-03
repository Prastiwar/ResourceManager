using MediatR;
using RPGDataEditor.Connection;
using RPGDataEditor.Mvvm.Services;
using RPGDataEditor.Services;

namespace RPGDataEditor.Mvvm
{
    public class ViewModelContext
    {
        public ViewModelContext(IMediator mediator,
                                IAppPersistanceService persistance,
                                IConnectionConfiguration connection,
                                IDialogService dialogService,
                                ILogger logger)
        {
            Mediator = mediator;
            Persistance = persistance;
            Connection = connection;
            DialogService = dialogService;
            Logger = logger;
        }

        public IMediator Mediator { get; }
        public IAppPersistanceService Persistance { get; }
        public IConnectionConfiguration Connection { get; }
        public IDialogService DialogService { get; }
        public ILogger Logger { get; }
    }
}
