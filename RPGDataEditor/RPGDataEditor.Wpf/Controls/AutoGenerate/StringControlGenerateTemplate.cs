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
        public StringControlGenerateTemplate(PropertyInfo info) : base(info) { }

        public override DependencyObject LoadContent()
        {
            TextBox box = new TextBox() {
                Margin = new Thickness(5)
            };
            box.SetBinding(FrameworkElement.DataContextProperty, new Binding("Model"));
            box.SetBinding(TextBox.TextProperty, new Binding(Info.Name) { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            string friendlyName = Info.Name.MakeFriendlyName();
            HintAssist.SetHint(box, friendlyName);
            box.SetResourceReference(FrameworkElement.StyleProperty, "MaterialDesignFloatingHintTextBox");
            BehaviorCollection behaviours = Interaction.GetBehaviors(box);
            behaviours.Add(new CatchValidationBehavior());
            return box;
        }
    }
}
