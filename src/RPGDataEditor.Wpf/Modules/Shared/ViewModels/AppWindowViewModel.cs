using Prism.Commands;
using RPGDataEditor.Core;
using RPGDataEditor.Mvvm;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RPGDataEditor.Wpf.ViewModels
{
    public class AppWindowViewModel : ViewModelBase
    {
        public AppWindowViewModel(ViewModelContext context, AppVersionChecker versionChecker) : base(context) => VersionChecker = versionChecker;

        protected AppVersionChecker VersionChecker { get; }

        private string title = "RPG Data Editor";
        public string Title {
            get => title;
            set => SetProperty(ref title, value);
        }

        public ICommand loadedCommand;
        public ICommand LoadedCommand => loadedCommand ??= new DelegateCommand(OnWindowLoaded);

        private async void OnWindowLoaded() => await OnWindowLoadedAsync();

        protected virtual async Task OnWindowLoadedAsync()
        {
            bool isUpdated = await VersionChecker.CheckVersionAsync();
            if (!isUpdated)
            {
                await Context.DialogService.ShowDialogAsync(DialogNames.UpdateDialog);
                Application.Current.Shutdown();
            }
        }
    }
}
