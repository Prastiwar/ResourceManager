using RPGDataEditor.Core.Validation;
using System.Collections.Generic;
using System.Windows;

namespace RPGDataEditor.Wpf
{
    public class AttachProperties : DependencyObject
    {
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.RegisterAttached("IsLoading", typeof(bool), typeof(AttachProperties), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));
        public static void SetIsLoading(UIElement element, bool value) => element.SetValue(IsLoadingProperty, value);
        public static bool GetIsLoading(UIElement element) => (bool)element.GetValue(IsLoadingProperty);

        public static readonly DependencyProperty ValidableObjectProperty =
            DependencyProperty.RegisterAttached("ValidableObject", typeof(IValidable), typeof(AttachProperties));
        public static void SetValidableObject(UIElement element, IValidable value) => element.SetValue(ValidableObjectProperty, value);
        public static IValidable GetValidableObject(UIElement element) => (IValidable)element.GetValue(ValidableObjectProperty);

        public static readonly DependencyProperty ValidablePathFormatProperty =
            DependencyProperty.RegisterAttached("ValidablePathFormat", typeof(string), typeof(AttachProperties));
        public static void SetValidablePathFormat(UIElement element, string value) => element.SetValue(ValidablePathFormatProperty, value);
        public static string GetValidablePathFormat(UIElement element) => (string)element.GetValue(ValidablePathFormatProperty);

        public static readonly DependencyProperty ValidablePathValuesProperty =
            DependencyProperty.RegisterAttached("ValidablePathValues", typeof(IList<object>), typeof(AttachProperties));
        public static void SetValidablePathValues(UIElement element, IList<object> value) => element.SetValue(ValidablePathValuesProperty, value);
        public static IList<object> GetValidablePathValues(UIElement element) => (IList<object>)element.GetValue(ValidablePathValuesProperty);
    }
}
