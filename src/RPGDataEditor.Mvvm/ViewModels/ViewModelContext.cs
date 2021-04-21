using MediatR;
using RPGDataEditor.Mvvm.Services;
using RPGDataEditor.Services;

namespace RPGDataEditor.Mvvm
{
    public class ViewModelContext
    {
        public ViewModelContext(IMediator mediator,
                                IDialogService dialogService,
                                ILogger logger)
        {
            Mediator = mediator;
            DialogService = dialogService;
            Logger = logger;
        }

        public IMediator Mediator { get; }
        public IDialogService DialogService { get; }
        public ILogger Logger { get; }
    }
}
