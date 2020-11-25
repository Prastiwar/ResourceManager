using Prism.Services.Dialogs;
using RPGDataEditor.Core.Validation;

namespace RPGDataEditor.Core.Mvvm
{
    public class ViewModelContext
    {
        public ViewModelContext(SessionContext session, IDialogService dialogService, IValidationProvider validationProvider)
        {
            Session = session;
            DialogService = dialogService;
            ValidationProvider = validationProvider;
        }

        public SessionContext Session { get; }
        public IDialogService DialogService { get; }
        public IValidationProvider ValidationProvider { get; }
    }
}
