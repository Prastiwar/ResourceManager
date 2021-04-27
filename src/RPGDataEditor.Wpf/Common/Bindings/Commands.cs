using MaterialDesignThemes.Wpf;
using Prism.Commands;
using RPGDataEditor.Extensions.Prism.Wpf;
using RPGDataEditor.Models;
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
        public static ICommand RemoveListItemCommand(Func<IList> getList) => new DelegateCommand<object>(item => getList().Remove(item));
        public static ICommand RemoveListItemCommand<T>(Func<IList<T>> getList) => new DelegateCommand<T>(item => getList().Remove(item));

        public static ICommand AddParameterListItemCommand<T>() => AddParameterListItemCommand(() => Activator.CreateInstance<T>());

        public static ICommand AddParameterListItemCommand<T>(Func<T> createInstance) => new DelegateCommand<IList>((x) => x.Add(createInstance()));

        public static ICommand AddListItemCommand(Func<IList> getList, Func<object> createInstance) => new DelegateCommand(() => getList().Add(createInstance()));

        public static ICommand AddListItemCommand<T>(Func<IList<T>> getList) => AddListItemCommand(getList, () => Activator.CreateInstance<T>());
        public static ICommand AddListItemCommand<T>(Func<IList<T>> getList, Func<T> createInstance) => new DelegateCommand(() => getList().Add(createInstance()));

        /// <summary> Gets Button (sender) as parameter and gets ListView from its Tag to remove item retrieved from button's DataContext </summary>
        public static ICommand RemoveItemFromListView() => new DelegateCommand<Button>(x => {
            if (x.Tag is ListView listView)
            {
                if (listView.ItemsSource is IList list)
                {
                    list.Remove(x.DataContext);
                }
            }
        });

        /// <summary> Gets FrameworkElement as parameter and toggles its visibility </summary>
        public static ICommand ToggleVisibilityCommand => new DelegateCommand<FrameworkElement>(x => x.Visibility = x.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible);

        public static void ToggleExpandIcon(Button btn)
        {
            if (btn.Content is PackIcon icon)
            {
                icon.Kind = icon.Kind == PackIconKind.ExpandMore ? PackIconKind.ExpandLess : PackIconKind.ExpandMore;
            }
        }

        public static ICommand AddRequirementCommand {
            get {
                return AddParameterListItemCommand(() => Application.Current.TryResolve<IImplementationProvider<Requirement>>().Get());
            }
        }
    }
}
