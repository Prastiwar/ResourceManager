using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace RPGDataEditor.Wpf.Controls
{
    [ContentProperty(nameof(ItemContentTemplate))]
    public class ListDataCard : UserControl
    {
        public static DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IList), typeof(ListDataCard),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public IList ItemsSource {
            get => (IList)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static DependencyProperty AddItemCommandProperty =
            DependencyProperty.Register(nameof(AddItemCommand), typeof(ICommand), typeof(ListDataCard));
        public ICommand AddItemCommand {
            get => (ICommand)GetValue(AddItemCommandProperty);
            set => SetValue(AddItemCommandProperty, value);
        }

        public static DependencyProperty AddItemCommandParameterProperty =
            DependencyProperty.Register(nameof(AddItemCommandParameter), typeof(object), typeof(ListDataCard));
        public object AddItemCommandParameter {
            get => GetValue(AddItemCommandParameterProperty);
            set => SetValue(AddItemCommandParameterProperty, value);
        }

        public static DependencyProperty RemoveItemCommandProperty =
            DependencyProperty.Register(nameof(RemoveItemCommand), typeof(ICommand), typeof(ListDataCard));
        public ICommand RemoveItemCommand {
            get => (ICommand)GetValue(RemoveItemCommandProperty);
            set => SetValue(RemoveItemCommandProperty, value);
        }

        public static DependencyProperty ItemContentTemplateProperty =
            DependencyProperty.Register(nameof(ItemContentTemplate), typeof(DataTemplate), typeof(ListDataCard));
        public DataTemplate ItemContentTemplate {
            get => (DataTemplate)GetValue(ItemContentTemplateProperty);
            set => SetValue(ItemContentTemplateProperty, value);
        }

        public static DependencyProperty HeaderTextProperty =
            DependencyProperty.Register(nameof(HeaderText), typeof(string), typeof(ListDataCard));
        public string HeaderText {
            get => (string)GetValue(HeaderTextProperty);
            set => SetValue(HeaderTextProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            OnTemplateApplied();
        }

        protected virtual void OnTemplateApplied()
        {
            if (RemoveItemCommand == null && GetBindingExpression(RemoveItemCommandProperty) == null)
            {
                RemoveItemCommand = Commands.RemoveListItemCommand(() => ItemsSource);
            }
            if (AddItemCommand == null && GetBindingExpression(AddItemCommandProperty) == null)
            {
                //AddItemCommand = Commands.AddListItemCommand(() => Requirements);
            }
        }

    }
}
