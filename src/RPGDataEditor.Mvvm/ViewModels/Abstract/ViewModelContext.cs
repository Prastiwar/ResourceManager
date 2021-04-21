using MediatR;
using RPGDataEditor.Core.Validation;
using RPGDataEditor.Mvvm.Services;
using RPGDataEditor.Services;

namespace RPGDataEditor.Mvvm
{
    public class ViewModelContext
    {
        public ViewModelContext(IMediator mediator,
                                IDialogService dialogService,
                                IValidationProvider validationProvider,
                                ISnackbarService snackbarService)
        {
            Mediator = mediator;
            DialogService = dialogService;
            ValidationProvider = validationProvider;
            SnackbarService = snackbarService;
        }

        public IMediator Mediator { get; }
        public IDialogService DialogService { get; }
        public IValidationProvider ValidationProvider { get; }
        public ISnackbarService SnackbarService { get; }
    }
}
