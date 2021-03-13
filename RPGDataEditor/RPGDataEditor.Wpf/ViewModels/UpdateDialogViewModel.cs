using Prism.Services.Dialogs;
using RPGDataEditor.Core.Mvvm;
using System.Threading.Tasks;

namespace RPGDataEditor.Wpf.ViewModels
{
    public class UpdateDialogViewModel : DialogViewModelBase
    {
        public UpdateDialogViewModel(ViewModelContext context) : base(context) { }

        public override string Title => "Update found";

        protected override Task InitializeAsync(IDialogParameters parameters) => Task.CompletedTask;
    }
}
