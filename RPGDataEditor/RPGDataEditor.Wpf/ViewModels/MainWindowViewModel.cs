using Prism.Commands;
using RPGDataEditor.Core;
using RPGDataEditor.Core.Mvvm;
using System.Windows;
using System.Windows.Input;

namespace RPGDataEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(ViewModelContext context, AppVersionChecker versionChecker) : base(context) => this.versionChecker = versionChecker;

        private readonly AppVersionChecker versionChecker;

        private string title = "RPG Data Editor";
        public string Title {
            get => title;
            set => SetProperty(ref title, value);
        }

        public ICommand loadedCommand;
        public ICommand LoadedCommand => loadedCommand ??= new DelegateCommand(OnWindowLoaded);

        private async void OnWindowLoaded()
        {
            bool isUpdated = await versionChecker.CheckVersionAsync();
            if (!isUpdated)
            {
                await Context.DialogService.ShowDialogAsync("UpdateDialog");
                Application.Current.Shutdown();
            }
        }
    }
}
