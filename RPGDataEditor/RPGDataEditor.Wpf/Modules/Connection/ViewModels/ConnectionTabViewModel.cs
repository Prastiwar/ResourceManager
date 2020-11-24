using Prism.Regions;
using RPGDataEditor.Core.Mvvm;
using System.Threading.Tasks;

namespace RPGDataEditor.Wpf.Connection.ViewModels
{
    public class ConnectionTabViewModel : TabViewModel
    {
        public ConnectionTabViewModel(SessionContext context) : base(context) { }

        public override Task<bool> CanSwitchTo(NavigationContext navigationContext) => Task.FromResult(true);

        public override async Task<bool> CanSwitchFrom(NavigationContext navigationContext)
        {
            bool isValid = await Context.IsValidAsync();
            if (!isValid)
            {
                // TODO: Show alert to set proper connection
            }
            else
            {
                Context.SaveSession(App.SessionFilePath);
            }
            return isValid;
        }
    }
}
