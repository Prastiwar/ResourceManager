using Prism.Services.Dialogs;
using RPGDataEditor.Core.Services;
using RPGDataEditor.Core.Validation;

namespace RPGDataEditor.Core.Mvvm
{
    public class ViewModelContext
    {
        public ViewModelContext(ISessionContext session,
                                IConnectionService connectionService,
                                IDialogService dialogService,
                                IValidationProvider validationProvider,
                                ISnackbarService snackbarService)
        {
            Session = session;
            ConnectionService = connectionService;
            DialogService = dialogService;
            ValidationProvider = validationProvider;
            SnackbarService = snackbarService;
        }

        public ISessionContext Session { get; }
        public IConnectionService ConnectionService { get; }
        public IDialogService DialogService { get; }
        public IValidationProvider ValidationProvider { get; }
        public ISnackbarService SnackbarService { get; }
    }
}
