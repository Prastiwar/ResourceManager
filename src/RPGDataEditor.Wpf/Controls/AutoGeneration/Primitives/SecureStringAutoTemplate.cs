using MaterialDesignThemes.Wpf;
using RPGDataEditor.Wpf.Behaviors;
using System.Reflection;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace RPGDataEditor.Wpf.Controls
{
    public class SecureStringAutoTemplate : AutoTemplate<SecureString>
    {
        public override DependencyObject LoadContent(PropertyInfo info)
        {
            PasswordBox box = new PasswordBox {
                Margin = new Thickness(5)
            };
            PasswordBoxHelper.SetBindPassword(box, true);
            box.SetBinding(PasswordBoxHelper.SecurePasswordProperty, new Binding(info.Name));
            HintAssist.SetHint(box, info.Name.MakeFriendlyName());
            box.SetResourceReference(FrameworkElement.StyleProperty, "MaterialDesignFloatingHintPasswordBox");
            BehaviorCollection behaviours = Interaction.GetBehaviors(box);
            behaviours.Add(new ValidationListenerBehavior());
            return box;
        }
    }
}
