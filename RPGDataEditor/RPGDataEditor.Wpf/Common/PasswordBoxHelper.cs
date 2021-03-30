using System.Windows;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf
{
    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordBoxHelper), new PropertyMetadata(string.Empty, OnPasswordChanged));
        public static string GetPassword(DependencyObject dp) => (string)dp.GetValue(PasswordProperty);
        public static void SetPassword(DependencyObject dp, string value) => dp.SetValue(PasswordProperty, value);

        public static readonly DependencyProperty BindPasswordProperty = DependencyProperty.RegisterAttached(
            "BindPassword", typeof(bool), typeof(PasswordBoxHelper), new PropertyMetadata(false, OnBindPasswordChanged));
        public static void SetBindPassword(DependencyObject dp, bool value) => dp.SetValue(BindPasswordProperty, value);
        public static bool GetBindPassword(DependencyObject dp) => (bool)dp.GetValue(BindPasswordProperty);

        private static readonly DependencyProperty IsUpdatingPasswordProperty =
            DependencyProperty.RegisterAttached("IsUpdatingPassword", typeof(bool), typeof(PasswordBoxHelper), new PropertyMetadata(false));
        private static bool GetIsUpdatingPassword(DependencyObject dp) => (bool)dp.GetValue(IsUpdatingPasswordProperty);
        private static void SetIsUpdatingPassword(DependencyObject dp, bool value) => dp.SetValue(IsUpdatingPasswordProperty, value);

        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox box = d as PasswordBox;
            if (d == null || !GetBindPassword(d))
            {
                return;
            }
            box.PasswordChanged -= HandlePasswordChanged;
            if (!GetIsUpdatingPassword(box))
            {
                box.Password = (string)e.NewValue;
            }
            box.PasswordChanged += HandlePasswordChanged;
        }

        private static void OnBindPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            if (dp is PasswordBox box)
            {
                bool shouldBind = (bool)(e.NewValue);
                box.PasswordChanged -= HandlePasswordChanged;
                if (shouldBind)
                {
                    box.PasswordChanged += HandlePasswordChanged;
                }
            }
        }

        private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox box = sender as PasswordBox;
            SetIsUpdatingPassword(box, true);
            SetPassword(box, box.Password);
            SetIsUpdatingPassword(box, false);
        }
    }
}