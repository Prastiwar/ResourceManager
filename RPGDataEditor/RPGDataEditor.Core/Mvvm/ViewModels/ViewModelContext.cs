using Prism.Services.Dialogs;
using RPGDataEditor.Core.Services;
using RPGDataEditor.Core.Providers;
using RPGDataEditor.Core.Validation;

namespace RPGDataEditor.Core.Mvvm
{
    public class ViewModelContext
    {
        public ViewModelContext(SessionContext session,
                                IResourceProvider resourceProvider,
                                IDialogService dialogService,
                                IValidationProvider validationProvider,
                                ISnackbarService snackbarService)
        {
            Session = session;
            ResourceProvider = resourceProvider;
            DialogService = dialogService;
            ValidationProvider = validationProvider;
            SnackbarService = snackbarService;
        }

        public SessionContext Session { get; }
        public IResourceProvider ResourceProvider { get; }
        public IDialogService DialogService { get; }
        public IValidationProvider ValidationProvider { get; }
        public ISnackbarService SnackbarService { get; }
    }
}
