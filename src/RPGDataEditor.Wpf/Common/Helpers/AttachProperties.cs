using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RPGDataEditor.Wpf
{
    public class AttachProperties : DependencyObject
    {
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.RegisterAttached("Data", typeof(object), typeof(AttachProperties));
        public static void SetData(UIElement element, object value) => element.SetValue(DataProperty, value);
        public static object GetData(UIElement element) => element.GetValue(DataProperty);

        public static readonly DependencyProperty BindingProperty =
            DependencyProperty.RegisterAttached("Binding", typeof(Binding), typeof(AttachProperties));
        public static void SetBinding(UIElement element, Binding value) => element.SetValue(BindingProperty, value);
        public static Binding GetBinding(UIElement element) => BindingOperations.GetBindingExpression(element, BindingProperty).ParentBinding;

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.RegisterAttached("IsLoading", typeof(bool), typeof(AttachProperties), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));
        public static void SetIsLoading(UIElement element, bool value) => element.SetValue(IsLoadingProperty, value);
        public static bool GetIsLoading(UIElement element) => (bool)element.GetValue(IsLoadingProperty);

        public static readonly DependencyProperty AlternationIndexProperty =
            DependencyProperty.RegisterAttached("AlternationIndex", typeof(int), typeof(AttachProperties), new PropertyMetadata(-1));
        public static int GetAlternationIndex(UIElement element)
        {
            if (element is ListBoxItem)
            {
                return ItemsControl.GetAlternationIndex(element);
            }
            if (element is FrameworkElement el)
            {
                el.SetBinding(AlternationIndexProperty, new Binding(".") { RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(ListBoxItem), 1) });
                return (int)el.GetValue(AlternationIndexProperty);
            }
            return -1;
        }
    }
}
