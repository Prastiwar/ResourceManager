using AutoUpdaterDotNET;
using Prism.Commands;
using ResourceManager.Mvvm;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ResourceManager.Wpf.ViewModels
{
    public class AppWindowViewModel : BindableClass
    {
        private string title = "RPG Data Editor";
        public string Title {
            get => title;
            set => SetProperty(ref title, value);
        }

        public ICommand loadedCommand;
        public ICommand LoadedCommand => loadedCommand ??= new DelegateCommand(OnWindowLoaded);

        private async void OnWindowLoaded() => await OnWindowLoadedAsync();

        protected virtual Task OnWindowLoadedAsync()
        {
            AutoUpdater.Start();
            return Task.CompletedTask;
        }
    }
}
