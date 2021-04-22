using RPGDataEditor.Core;
using RPGDataEditor.Mvvm;

namespace RPGDataEditor.Wpf.ViewModels
{
    public class MainWindowViewModel : AppWindowViewModel
    {
        public MainWindowViewModel(ViewModelContext context, AppVersionChecker versionChecker) : base(context, versionChecker) { }
    }
}
