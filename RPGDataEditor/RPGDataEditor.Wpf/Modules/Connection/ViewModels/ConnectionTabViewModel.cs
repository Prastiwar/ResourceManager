using FluentValidation.Results;
using Prism.Regions;
using RPGDataEditor.Core.Mvvm;
using System.Threading.Tasks;

namespace RPGDataEditor.Wpf.Connection.ViewModels
{
    public class ConnectionTabViewModel : TabViewModel
    {
        public ConnectionTabViewModel(ViewModelContext context) : base(context) { }

        public override Task<bool> CanSwitchTo(NavigationContext navigationContext) => Task.FromResult(true);

        public override async Task<bool> CanSwitchFrom(NavigationContext navigationContext)
        {
            ValidationResult result = await Context.ValidationProvider.ValidateAsync(Session);
            if (result.IsValid)
            {
                Session.SaveSession(App.SessionFilePath);
            }
            return result.IsValid;
        }
    }
}
