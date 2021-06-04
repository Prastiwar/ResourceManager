using MaterialDesignThemes.Wpf;
using RPGDataEditor.Wpf.Behaviors;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace RPGDataEditor.Wpf.Controls
{
    public class SecureStringAutoTemplate : AutoTemplate<SecureString>
    {
        public override DependencyObject LoadContent(object context, TemplateOptions options)
        {
            PasswordBox box = new PasswordBox {
                Margin = new Thickness(5)
            };
            PasswordBoxHelper.SetBindPassword(box, true);
            box.SetBinding(PasswordBoxHelper.SecurePasswordProperty, new Binding(options.BindingName));
            HintAssist.SetHint(box, options.BindingName.MakeFriendlyName());
            box.SetResourceReference(FrameworkElement.StyleProperty, "MaterialDesignFloatingHintPasswordBox");
            BehaviorCollection behaviours = Interaction.GetBehaviors(box);
            behaviours.Add(new ValidationListenerBehaviorView());
            return box;
        }
    }
}
