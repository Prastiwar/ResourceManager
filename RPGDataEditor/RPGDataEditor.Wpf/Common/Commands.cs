using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace RPGDataEditor.Wpf
{
    public static class Commands
    {
        public static ICommand RemoveListItemLiCommand<T>(Func<IList<T>> getList) => new DelegateCommand<T>(item => getList().Remove(item));

        public static ICommand AddListItemLiCommand<T>(Func<IList<T>> getList) => AddListItemLiCommand(getList, () => Activator.CreateInstance<T>());

        public static ICommand AddListItemLiCommand<T>(Func<IList<T>> getList, Func<T> createInstance) => new DelegateCommand(() => getList().Add(createInstance()));

        public static ICommand ToggleVisibilityCommand => new DelegateCommand<FrameworkElement>(x => x.Visibility = x.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible);
    }
}
