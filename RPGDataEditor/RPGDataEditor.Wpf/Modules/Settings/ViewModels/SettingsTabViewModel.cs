using Prism.Regions;
using RPGDataEditor.Core.Mvvm;
using System.Threading.Tasks;

namespace RPGDataEditor.Wpf.Settings.ViewModels
{
    public class SettingsTabViewModel : TabViewModel
    {
        public SettingsTabViewModel(ViewModelContext context) : base(context) { }

        public override Task<bool> CanSwitchTo(NavigationContext navigationContext) => Task.FromResult(true);

    }
}
