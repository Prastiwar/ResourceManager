using RPGDataEditor.Wpf.Behaviors;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace RPGDataEditor.Wpf.Controls
{
    public class IntAutoTemplate : NumericAutoTemplate<int> { }

    public class DoubleAutoTemplate : NumericAutoTemplate<double> { }

    public class FloatAutoTemplate : NumericAutoTemplate<float> { }

    public class NumericAutoTemplate<T> : AutoTemplate<T>
    {
        public override DependencyObject LoadContent(PropertyInfo info)
        {
            TextBox box = new TextBox() {
                Margin = new Thickness(5),
                InputScope = new InputScopeConverter().ConvertFrom(InputScopeNameValue.Number) as InputScope
            };
            box.SetBinding(TextBox.TextProperty, new Binding(info.Name) {
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                StringFormat = Type == typeof(int) ? null : "{0:0.0#}"
            });
            box.SetResourceReference(FrameworkElement.StyleProperty, "MaterialDesignFloatingHintTextBox");
            BehaviorCollection behaviours = Interaction.GetBehaviors(box);
            behaviours.Add(new CatchValidationBehavior());
            behaviours.Add(new NumericFieldBehaviour() {
                EmptyValue = Type == typeof(int) ? "0" : "0.0",
                IsPrecise = Type != typeof(int)
            });
            return box;
        }
    }
}
