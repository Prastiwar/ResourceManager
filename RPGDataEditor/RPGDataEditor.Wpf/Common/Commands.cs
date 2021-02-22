using MaterialDesignThemes.Wpf;
using Prism.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RPGDataEditor.Wpf
{
    public static class Commands
    {
        public static ICommand RemoveListItemLiCommand<T>(Func<IList<T>> getList) => new DelegateCommand<T>(item => getList().Remove(item));

        public static ICommand AddParameterListItemLiCommand<T>() => AddParameterListItemLiCommand(() => Activator.CreateInstance<T>());

        public static ICommand AddParameterListItemLiCommand<T>(Func<T> createInstance) => new DelegateCommand<IList>((x) => x.Add(createInstance()));

        public static ICommand AddListItemLiCommand<T>(Func<IList<T>> getList) => AddListItemLiCommand(getList, () => Activator.CreateInstance<T>());

        public static ICommand AddListItemLiCommand<T>(Func<IList<T>> getList, Func<T> createInstance) => new DelegateCommand(() => getList().Add(createInstance()));

        public static ICommand RemoveItemFromListView() => new DelegateCommand<Button>(x => {
            if (x.Tag is ListView listView)
            {
                if (listView.ItemsSource is IList list)
                {
                    list.Remove(x.DataContext);
                }
            }
        });

        public static ICommand ToggleVisibilityCommand => new DelegateCommand<FrameworkElement>(x => x.Visibility = x.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible);

        public static void ToggleExpandIcon(Button btn)
        {
            if (btn.Content is PackIcon icon)
            {
                icon.Kind = icon.Kind == PackIconKind.ExpandMore ? PackIconKind.ExpandLess : PackIconKind.ExpandMore;
            }
        }
    }
}
