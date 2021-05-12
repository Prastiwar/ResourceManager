using RPGDataEditor.Core.Validation;
using System.Collections.Generic;
using System.Windows;

namespace RPGDataEditor.Wpf
{
    public class ValidableHelper : DependencyObject
    {
        public static readonly DependencyProperty ValidableObjectProperty =
            DependencyProperty.RegisterAttached("ValidableObject", typeof(IValidable), typeof(ValidableHelper));
        public static void SetValidableObject(UIElement element, IValidable value) => element.SetValue(ValidableObjectProperty, value);
        public static IValidable GetValidableObject(UIElement element) => (IValidable)element.GetValue(ValidableObjectProperty);

        public static readonly DependencyProperty ValidablePathFormatProperty =
            DependencyProperty.RegisterAttached("ValidablePathFormat", typeof(string), typeof(ValidableHelper));
        public static void SetValidablePathFormat(UIElement element, string value) => element.SetValue(ValidablePathFormatProperty, value);
        public static string GetValidablePathFormat(UIElement element) => (string)element.GetValue(ValidablePathFormatProperty);

        public static readonly DependencyProperty ValidablePathValuesProperty =
            DependencyProperty.RegisterAttached("ValidablePathValues", typeof(IList<object>), typeof(ValidableHelper));
        public static void SetValidablePathValues(UIElement element, IList<object> value) => element.SetValue(ValidablePathValuesProperty, value);
        public static IList<object> GetValidablePathValues(UIElement element) => (IList<object>)element.GetValue(ValidablePathValuesProperty);

        public static readonly DependencyProperty ValidableHookProperty =
            DependencyProperty.RegisterAttached("ValidableHook", typeof(IValidationHook), typeof(ValidableHelper));
        public static void SetValidableHook(DependencyObject element, IValidationHook value) => element.SetValue(ValidableHookProperty, value);
        public static IValidationHook GetValidableHook(DependencyObject element) => (IValidationHook)element.GetValue(ValidableHookProperty);

        public static bool HasHook(DependencyObject obj) => obj.HasBinding(ValidableHookProperty);
    }
}
