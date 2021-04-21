using RPGDataEditor.Mvvm.Navigation;
using System.Threading.Tasks;

namespace RPGDataEditor.Mvvm
{
    public class ScreenViewModel : ViewModelBase, INavigationAware
    {
        public ScreenViewModel(ViewModelContext context) : base(context) { }

        private bool isLoading;
        public bool IsLoading {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }

        public virtual Task OnNavigatedFromAsync(INavigationContext navigationContext) => Task.FromResult(true);

        public virtual Task OnNavigatedToAsync(INavigationContext navigationContext) => Task.FromResult(true);

        public virtual Task<bool> CanNavigateTo(INavigationContext navigationContext) => Context.ConnectionService.Client.ConnectAsync();

        public virtual Task<bool> CanNavigateFrom(INavigationContext navigationContext) => Task.FromResult(true);

        public virtual Task Refresh() => Task.CompletedTask;
    }
}
