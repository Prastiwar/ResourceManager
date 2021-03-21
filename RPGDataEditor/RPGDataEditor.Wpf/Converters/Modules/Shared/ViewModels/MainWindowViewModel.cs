using RPGDataEditor.Core;
using RPGDataEditor.Core.Mvvm;

namespace RPGDataEditor.ViewModels
{
    internal class MainWindowViewModel : AppWindowViewModel
    {
        public MainWindowViewModel(ViewModelContext context, AppVersionChecker versionChecker) : base(context, versionChecker) { }
    }
}
