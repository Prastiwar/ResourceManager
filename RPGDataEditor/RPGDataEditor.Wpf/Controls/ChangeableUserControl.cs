using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace RPGDataEditor.Wpf.Controls
{
    [ContentProperty(nameof(TypesSource))]
    public class ChangeableUserControl : UserControl
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

            typeComboBox.SelectionChanged += OnTypeComboBoxSelectionChanged;
        }

        private void OnTypeComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                if (e.AddedItems[0] is ComboBoxItem selected)
                {
                    ChangeTypeCommand?.Execute(ChangeTypeCommandParameter);
                    ChangeTypeEventArgs changeTypeArgs = new ChangeTypeEventArgs(DataContext, selected.Name) {
                        RoutedEvent = TypeChangeEvent
                    };
                    RaiseEvent(changeTypeArgs);
                    ApplyActualContent(selected.Name);
                }
            }
        }

        protected virtual void ApplyActualContent(string name)
        {
            object content = GetActualContentResource(name);
            actualContent.Content = content;
        }

        protected virtual object GetActualContentResource(string name) => Application.Current.TryFindResource(name + "ChangeableContent");
    }
}
