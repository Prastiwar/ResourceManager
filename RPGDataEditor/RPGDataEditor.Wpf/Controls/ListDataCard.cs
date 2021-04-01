using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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

        public static DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(nameof(HeaderTemplate), typeof(DataTemplate), typeof(ListDataCard));
        public DataTemplate HeaderTemplate {
            get => (DataTemplate)GetValue(HeaderTemplateProperty);
            set => SetValue(HeaderTemplateProperty, value);
        }

        public static DependencyProperty HeaderTextProperty =
            DependencyProperty.Register(nameof(HeaderText), typeof(string), typeof(ListDataCard));
        public string HeaderText {
            get => (string)GetValue(HeaderTextProperty);
            set => SetValue(HeaderTextProperty, value);
        }

        public static DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(ListDataCard));
        public bool IsReadOnly {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public static DependencyProperty IsExpandableProperty =
            DependencyProperty.Register(nameof(IsExpandable), typeof(bool), typeof(ListDataCard), new PropertyMetadata(true));
        public bool IsExpandable {
            get => (bool)GetValue(IsExpandableProperty);
            set => SetValue(IsExpandableProperty, value);
        }

        public static DependencyProperty IsExpandedProperty =
            DependencyProperty.Register(nameof(IsExpanded), typeof(bool), typeof(ListDataCard), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool IsExpanded {
            get => (bool)GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }

        public static DependencyProperty NoExpandableVisibilityProperty =
            DependencyProperty.Register(nameof(NoExpandableVisibility), typeof(bool), typeof(ListDataCard));
        public bool NoExpandableVisibility {
            get => (bool)GetValue(NoExpandableVisibilityProperty);
            set => SetValue(NoExpandableVisibilityProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            OnTemplateApplied();
        }

        protected virtual void OnTemplateApplied()
        {
            if (HeaderTemplate == null && GetBindingExpression(HeaderTemplateProperty) == null)
            {
                HeaderTemplate = TemplateGenerator.CreateDataTemplate(() => {
                    TextBlock textBlock = new TextBlock();
                    textBlock.SetBinding(TextBlock.TextProperty, new Binding(nameof(HeaderText)) { Source = this });
                    return textBlock;
                });
            }
            if (RemoveItemCommand == null && GetBindingExpression(RemoveItemCommandProperty) == null)
            {
                RemoveItemCommand = Commands.RemoveListItemCommand(() => ItemsSource);
            }
        }

    }
}
