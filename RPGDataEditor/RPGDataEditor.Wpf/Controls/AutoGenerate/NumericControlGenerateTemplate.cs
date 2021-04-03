using MaterialDesignThemes.Wpf;
using RPGDataEditor.Core;
using RPGDataEditor.Wpf.Behaviors;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace RPGDataEditor.Wpf.Controls
{
    public class IntControlGenerateTemplate : NumericControlGenerateTemplate<int>
    {
        public IntControlGenerateTemplate(PropertyInfo info) : base(info) { }
    }

    public class DoubleControlGenerateTemplate : NumericControlGenerateTemplate<double>
    {
        public DoubleControlGenerateTemplate(PropertyInfo info) : base(info) { }
    }

    public class FloatControlGenerateTemplate : NumericControlGenerateTemplate<float>
    {
        public FloatControlGenerateTemplate(PropertyInfo info) : base(info) { }
    }

    public class NumericControlGenerateTemplate<T> : ControlGenerateTemplate<T>
    {
        public NumericControlGenerateTemplate(PropertyInfo info) : base(info) { }

        public override DependencyObject LoadContent()
        {
            TextBox box = new TextBox() {
                Margin = new Thickness(5),
                InputScope = new InputScopeConverter().ConvertFrom(InputScopeNameValue.Number) as InputScope
            };
            box.SetBinding(TextBox.TextProperty, new Binding(Info.Name) { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            box.SetBinding(FrameworkElement.DataContextProperty, new Binding("Model"));
            box.SetResourceReference(FrameworkElement.StyleProperty, "MaterialDesignFloatingHintTextBox");
            string friendlyName = Info.Name.MakeFriendlyName();
            HintAssist.SetHint(box, friendlyName);
            BehaviorCollection behaviours = Interaction.GetBehaviors(box);
            behaviours.Add(new CatchValidationBehavior());
            behaviours.Add(new NumericFieldBehaviour() { EmptyValue = Type == typeof(int) ? "0" : "0.0" });
            return box;
        }

    }
}
