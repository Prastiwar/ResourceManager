using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace RPGDataEditor.Wpf.Controls
{
    [ContentProperty(nameof(TypesSource))]
    public abstract class ChangeableUserControl : UserControl
    {
        public delegate void ChangeTypeEventHandler(object sender, ChangeTypeEventArgs e);

        public class ChangeTypeEventArgs : RoutedEventArgs
        {
            public ChangeTypeEventArgs(object item, string targetType)
            {
                Item = item;
                TargetType = targetType;
            }

            public object Item { get; }
            public string TargetType { get; }
        }

        public static DependencyProperty HintTextProperty =
            DependencyProperty.Register(nameof(HintText), typeof(string), typeof(ChangeableUserControl), new PropertyMetadata("Type"));
        public string HintText {
            get => (string)GetValue(HintTextProperty);
            set => SetValue(HintTextProperty, value);
        }

        public static DependencyProperty TypesSourceProperty =
            DependencyProperty.Register(nameof(TypesSource), typeof(IEnumerable<object>), typeof(ChangeableUserControl));
        public IEnumerable<object> TypesSource {
            get => (IEnumerable<object>)GetValue(TypesSourceProperty);
            set => SetValue(TypesSourceProperty, value);
        }

        public static DependencyProperty ChangeTypeRequestProperty =
            DependencyProperty.Register(nameof(ChangeTypeCommand), typeof(ICommand), typeof(ChangeableUserControl));
        public ICommand ChangeTypeCommand {
            get => (ICommand)GetValue(ChangeTypeRequestProperty);
            set => SetValue(ChangeTypeRequestProperty, value);
        }

        public static DependencyProperty ChangeTypeCommandParameterProperty =
            DependencyProperty.Register(nameof(ChangeTypeCommandParameter), typeof(object), typeof(ChangeableUserControl));
        public object ChangeTypeCommandParameter {
            get => GetValue(ChangeTypeCommandParameterProperty);
            set => SetValue(ChangeTypeCommandParameterProperty, value);
        }

        public static readonly RoutedEvent TypeChangeEvent
            = EventManager.RegisterRoutedEvent("TypeChange", RoutingStrategy.Direct, typeof(ChangeTypeEventHandler), typeof(ChangeableUserControl));

        public event ChangeTypeEventHandler TypeChange {
            add => AddHandler(SizeChangedEvent, value, false);
            remove => RemoveHandler(SizeChangedEvent, value);
        }

        private ComboBox typeComboBox;
        private ContentPresenter actualContent;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            typeComboBox = Template.FindName("TypeComboBox", this) as ComboBox;
            actualContent = Template.FindName("ActualContent", this) as ContentPresenter;
            OnTemplateApplied();
        }

        protected virtual void OnTemplateApplied()
        {
            string name = GetDataContextItemName();
            if (!string.IsNullOrEmpty(name))
            {
                ApplyActualContent(name);
                typeComboBox.SelectedItem = typeComboBox.Items.Cast<object>().FirstOrDefault(item => CompareItem(item, name));
            }
            typeComboBox.SelectionChanged += OnTypeComboBoxSelectionChanged;
        }

        protected virtual bool CompareItem(object item, string name)
        {
            string str = null;
            if (item is ComboBoxItem boxItem)
            {
                str = boxItem.Name;
            }
            else
            {
                str = item.ToString();
            }
            return name.CompareTo(str) == 0;
        }

        protected abstract string GetDataContextItemName();

        protected virtual void ApplyActualContent(string name)
        {
            object content = GetActualContentResource(name);
            if (content is DataTemplate template)
            {
                content = template.LoadContent();
            }
            actualContent.Content = content;
        }

        protected virtual object GetActualContentResource(string name) => Application.Current.TryFindResource(name + "ChangeableContent");

        private void OnTypeComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                string name = null;
                if (e.AddedItems[0] is ComboBoxItem selected)
                {
                    name = selected.Name;
                }
                else
                {
                    name = e.AddedItems[0].ToString();
                }
                ChangeTypeCommand?.Execute(ChangeTypeCommandParameter);
                ChangeTypeEventArgs changeTypeArgs = new ChangeTypeEventArgs(DataContext, name) {
                    RoutedEvent = TypeChangeEvent
                };
                RaiseEvent(changeTypeArgs);
                ApplyActualContent(name);
            }
        }
    }
}
