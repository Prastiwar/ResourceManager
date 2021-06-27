using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace ResourceManager.Wpf
{
    public static class WindowHelper
    {
        private static readonly DependencyPropertyKey IsHiddenCloseButtonKey
            = DependencyProperty.RegisterAttachedReadOnly("IsHiddenCloseButton", typeof(bool), typeof(WindowHelper), new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty IsHiddenCloseButtonProperty = IsHiddenCloseButtonKey.DependencyProperty;

        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static bool GetIsHiddenCloseButton(Window obj) => (bool)obj.GetValue(IsHiddenCloseButtonProperty);
        private static void SetIsHiddenCloseButton(Window obj, bool value) => obj.SetValue(IsHiddenCloseButtonKey, value);

        public static readonly DependencyProperty HideCloseButtonProperty
            = DependencyProperty.RegisterAttached("HideCloseButton", typeof(bool), typeof(WindowHelper), new PropertyMetadata(false, HideCloseButtonChangedCallback));

        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static bool GetHideCloseButton(Window obj) => (bool)obj.GetValue(HideCloseButtonProperty);

        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static void SetHideCloseButton(Window obj, bool value) => obj.SetValue(HideCloseButtonProperty, value);

        private static void HideCloseButtonChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Window window = d as Window;
            if (window == null)
            {
                return;
            }

            bool hideCloseButton = (bool)e.NewValue;
            if (hideCloseButton && !GetIsHiddenCloseButton(window))
            {
                if (!window.IsLoaded)
                {
                    window.Loaded += HideWhenLoadedDelegate;
                }
                else
                {
                    HideCloseButton(window);
                }
                SetIsHiddenCloseButton(window, true);
            }
            else if (!hideCloseButton && GetIsHiddenCloseButton(window))
            {
                if (!window.IsLoaded)
                {
                    window.Loaded -= ShowWhenLoadedDelegate;
                }
                else
                {
                    ShowCloseButton(window);
                }
                SetIsHiddenCloseButton(window, false);
            }
        }

        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private static readonly RoutedEventHandler HideWhenLoadedDelegate = (sender, args) => {
            if (sender is Window == false)
            {
                return;
            }

            Window w = (Window)sender;
            HideCloseButton(w);
            w.Loaded -= HideWhenLoadedDelegate;
        };

        private static readonly RoutedEventHandler ShowWhenLoadedDelegate = (sender, args) => {
            if (sender is Window == false)
            {
                return;
            }

            Window w = (Window)sender;
            ShowCloseButton(w);
            w.Loaded -= ShowWhenLoadedDelegate;
        };

        private static void HideCloseButton(Window w)
        {
            IntPtr hwnd = new WindowInteropHelper(w).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private static void ShowCloseButton(Window w)
        {
            IntPtr hwnd = new WindowInteropHelper(w).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) | WS_SYSMENU);
        }
    }
}
