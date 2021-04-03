using MaterialDesignThemes.Wpf;
using RPGDataEditor.Core;
using RPGDataEditor.Wpf.Behaviors;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace RPGDataEditor.Wpf.Controls
{
    public class StringControlGenerateTemplate : ControlGenerateTemplate<string>
    {
        public override DependencyObject LoadContent(PropertyInfo info)
        {
            TextBox box = new TextBox() {
                Margin = new Thickness(5)
            };
            box.SetBinding(TextBox.TextProperty, new Binding(info.Name) { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            string friendlyName = info.Name.MakeFriendlyName();
            HintAssist.SetHint(box, friendlyName);
            box.SetResourceReference(FrameworkElement.StyleProperty, "MaterialDesignFloatingHintTextBox");
            BehaviorCollection behaviours = Interaction.GetBehaviors(box);
            behaviours.Add(new CatchValidationBehavior());
            return box;
        }
    }
}
