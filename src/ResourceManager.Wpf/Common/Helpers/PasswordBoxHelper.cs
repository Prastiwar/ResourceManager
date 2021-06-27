using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace ResourceManager.Wpf
{
    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordBoxHelper), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPasswordChanged));
        public static string GetPassword(DependencyObject dp) => (string)dp.GetValue(PasswordProperty);
        public static void SetPassword(DependencyObject dp, string value) => dp.SetValue(PasswordProperty, value);

        public static readonly DependencyProperty SecurePasswordProperty =
            DependencyProperty.RegisterAttached("SecurePassword", typeof(SecureString), typeof(PasswordBoxHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPasswordChanged));
        public static SecureString GetSecurePassword(DependencyObject dp) => (SecureString)dp.GetValue(SecurePasswordProperty);
        public static void SetSecurePassword(DependencyObject dp, SecureString value) => dp.SetValue(SecurePasswordProperty, value);

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
                if (e.NewValue is string password)
                {
                    box.Password = password;
                }
                else if (e.NewValue is SecureString securePassword)
                {
                    IntPtr valuePtr = IntPtr.Zero;
                    try
                    {
                        valuePtr = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                        box.Password = Marshal.PtrToStringUni(valuePtr);
                    }
                    finally
                    {
                        Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
                    }
                }
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
            SetSecurePassword(box, box.SecurePassword);
            SetIsUpdatingPassword(box, false);
        }
    }
}