using Microsoft.Extensions.Logging;
using Prism.Services.Dialogs;
using RPGDataEditor.Mvvm;
using System.Threading.Tasks;

namespace RPGDataEditor.Wpf.ViewModels
{
    public class UpdateDialogViewModel : DialogViewModelBase
    {
        public UpdateDialogViewModel(ILogger<UpdateDialogViewModel> logger) : base(logger) { }

        public override string Title => "Update found";

        protected override Task InitializeAsync(IDialogParameters parameters) => Task.CompletedTask;
    }
}
