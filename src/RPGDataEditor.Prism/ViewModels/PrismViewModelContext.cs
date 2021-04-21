using Prism.Services.Dialogs;
using RPGDataEditor.Core.Validation;
using RPGDataEditor.Services;

namespace RPGDataEditor.Mvvm
{
    public class PrismViewModelContext : ViewModelContext
    {
        public PrismViewModelContext(IConnectionService connectionService,
                                     IDialogService dialogService,
                                     IValidationProvider validationProvider,
                                     ISnackbarService snackbarService) : base(connectionService, validationProvider, snackbarService) => DialogService = dialogService;

        public IDialogService DialogService { get; }
    }
}
