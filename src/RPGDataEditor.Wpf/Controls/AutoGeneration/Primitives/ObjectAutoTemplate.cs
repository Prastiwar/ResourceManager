using MaterialDesignThemes.Wpf;
using RPGDataEditor.Wpf.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace RPGDataEditor.Wpf.Controls
{
    public class ObjectAutoTemplate : AutoTemplate<object>
    {
        public override DependencyObject LoadContent(object context, TemplateOptions options)
        {
            TextBox box = new TextBox() {
                Margin = new Thickness(5)
            };
            box.SetBinding(TextBox.TextProperty, new Binding(options.BindingName) { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            HintAssist.SetHint(box, options.BindingName.MakeFriendlyName());
            box.SetResourceReference(FrameworkElement.StyleProperty, "MaterialDesignFloatingHintTextBox");
            BehaviorCollection behaviours = Interaction.GetBehaviors(box);
            behaviours.Add(new ValidationListenerBehavior());
            return box;
        }
    }
}
