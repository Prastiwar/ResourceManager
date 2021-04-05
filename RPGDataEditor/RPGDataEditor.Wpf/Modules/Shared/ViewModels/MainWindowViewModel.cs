using RPGDataEditor.Core;
using RPGDataEditor.Core.Mvvm;

namespace RPGDataEditor.Wpf.ViewModels
{
    public class MainWindowViewModel : AppWindowViewModel
    {
        public MainWindowViewModel(ViewModelContext context, AppVersionChecker versionChecker) : base(context, versionChecker) { }
    }
}
