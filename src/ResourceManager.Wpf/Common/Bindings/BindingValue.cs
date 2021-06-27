using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace ResourceManager.Wpf
{
    [ContentProperty(nameof(Value))]
    public class BindingValue : DependencyObject
    {
        public static DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(Binding), typeof(BindingValue));
        public Binding Value {
            get {
                BindingExpression exp = BindingOperations.GetBindingExpression(this, ValueProperty);
                return exp?.ParentBinding;
            }
            set => SetValue(ValueProperty, value);
        }
    }
}
