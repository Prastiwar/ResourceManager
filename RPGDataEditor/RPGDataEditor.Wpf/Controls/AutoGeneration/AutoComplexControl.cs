using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Controls
{
    public class AutoComplexControl : AutoControl
    {
        protected override DependencyObject CreateContent()
        {
            object context = GetBindingExpression(PropertyContextProperty) != null ? PropertyContext : PropertyContext ?? DataContext;
            if (context == null)
            {
                return null;
            }
            DependencyObject root = Root as DependencyObject;
            if (root is Panel panel)
            {
                GenerateAndAddControlsTo(panel, context);
            }
            return root;
        }

        protected virtual void GenerateAndAddControlsTo(Panel panel, object context)
        {
            PropertyInfo[] properties = context.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                DependencyObject control = LoadControl(context, property.Name);
                if (control == null)
                {
                    // GenerateAndAddControlsTo(panel, property.PropertyType);
                }
                if (control is UIElement element)
                {
                    panel.Children.Add(element);
                }
            }
        }
    }
}
