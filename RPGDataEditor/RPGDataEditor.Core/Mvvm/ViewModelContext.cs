using Prism.Services.Dialogs;
using RPGDataEditor.Core.Services;
using RPGDataEditor.Core.Validation;

namespace RPGDataEditor.Core.Mvvm
{
    public class ViewModelContext
    {
        public ViewModelContext(SessionContext session, IDialogService dialogService, IValidationProvider validationProvider, ISnackbarService snackbarService)
        {
            Session = session;
            DialogService = dialogService;
            ValidationProvider = validationProvider;
            SnackbarService = snackbarService;
        }

        public SessionContext Session { get; }
        public IDialogService DialogService { get; }
        public IValidationProvider ValidationProvider { get; }
        public ISnackbarService SnackbarService { get; }
    }
}
