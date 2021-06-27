using MaterialDesignThemes.Wpf;
using ResourceManager.Wpf.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace ResourceManager.Wpf.Controls
{
    public class StringAutoTemplate : AutoTemplate<string>
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
