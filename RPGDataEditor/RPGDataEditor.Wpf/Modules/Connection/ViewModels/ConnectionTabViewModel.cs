using FluentValidation.Results;
using Prism.Regions;
using RPGDataEditor.Core;
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
                try
                {
                    Session.SaveSession(App.SessionFilePath);
                }
                catch (System.Exception ex)
                {
                    Logger.Error("Couldn't save session", ex);
                }
            }
            return result.IsValid;
        }

        public override Task OnNavigatedToAsync(NavigationContext navigationContext)
        {
            // TODO: Invoke on connection lost
            return Task.CompletedTask;
        }

        public override Task OnNavigatedFromAsync(NavigationContext navigationContext)
        {
            // TODO: Invoke on connection established
            return Task.CompletedTask;
        }
    }
}
