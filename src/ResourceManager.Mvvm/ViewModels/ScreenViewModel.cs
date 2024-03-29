﻿using ResourceManager.Mvvm.Navigation;
using System.Threading.Tasks;

namespace ResourceManager.Mvvm
{
    public class ScreenViewModel : BindableClass, INavigationAware
    {
        private bool isLoading;
        public bool IsLoading {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }

        public virtual Task OnNavigatedFromAsync(INavigationContext navigationContext) => Task.FromResult(true);

        public virtual Task OnNavigatedToAsync(INavigationContext navigationContext) => Task.FromResult(true);

        public virtual Task<bool> CanNavigateTo(INavigationContext navigationContext) => Task.FromResult(true);

        public virtual Task<bool> CanNavigateFrom(INavigationContext navigationContext) => Task.FromResult(true);

        public virtual Task Refresh() => Task.CompletedTask;
    }
}
