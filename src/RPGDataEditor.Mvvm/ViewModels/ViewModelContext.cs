using MediatR;
using RPGDataEditor.Connection;
using RPGDataEditor.Mvvm.Services;
using RPGDataEditor.Services;

namespace RPGDataEditor.Mvvm
{
    public class ViewModelContext
    {
        public ViewModelContext(IMediator mediator,
                                IConnectionSettings connection,
                                IDialogService dialogService,
                                ILogger logger)
        {
            Mediator = mediator;
            Connection = connection;
            DialogService = dialogService;
            Logger = logger;
        }

        public IMediator Mediator { get; }
        public IConnectionSettings Connection { get; }
        public IDialogService DialogService { get; }
        public ILogger Logger { get; }
    }
}
