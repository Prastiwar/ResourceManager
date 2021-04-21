using RPGDataEditor.Services;
using RPGDataEditor.Core.Validation;
using MediatR;

namespace RPGDataEditor.Mvvm
{
    public class ViewModelContext
    {
        public ViewModelContext(IConnectionService connectionService,
                                IValidationProvider validationProvider,
                                ISnackbarService snackbarService)
        {
            ConnectionService = connectionService;
            ValidationProvider = validationProvider;
            SnackbarService = snackbarService;
        }

        public IConnectionService ConnectionService { get; }
        public IValidationProvider ValidationProvider { get; }
        public ISnackbarService SnackbarService { get; }
        public IMediator Mediator { get; }
    }
}
