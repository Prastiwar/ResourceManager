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
            public ChangeTypeEventArgs(object item, TypeSource targetType)
            {
                Item = item;
                TargetType = targetType;
            }

            public object Item { get; }
            public TypeSource TargetType { get; }
        }

        public ChangeableUserControl() => DataContextChanged += OnDataContextChanged;

        public static DependencyProperty HintTextProperty =
            DependencyProperty.Register(nameof(HintText), typeof(string), typeof(ChangeableUserControl), new PropertyMetadata("Type"));
        public string HintText {
            get => (string)GetValue(HintTextProperty);
            set => SetValue(HintTextProperty, value);
        }

        public static DependencyProperty TypesSourceProperty =
            DependencyProperty.Register(nameof(TypesSource), typeof(IEnumerable<TypeSource>), typeof(ChangeableUserControl));
        public IEnumerable<TypeSource> TypesSource {
            get => (IEnumerable<TypeSource>)GetValue(TypesSourceProperty);
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
            add => AddHandler(TypeChangeEvent, value, false);
            remove => RemoveHandler(TypeChangeEvent, value);
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
            if (typeComboBox == null)
            {
                return;
            }
            typeComboBox.SelectionChanged -= OnTypeComboBoxSelectionChanged;
            TypeSource type = GetDataContextTypeSource();
            if (type != null)
            {
                ApplyActualContent(type);
                typeComboBox.SelectedItem = typeComboBox.Items.Cast<TypeSource>().FirstOrDefault(item => CompareItem(item, type));
            }
            typeComboBox.SelectionChanged += OnTypeComboBoxSelectionChanged;
        }

        protected virtual bool CompareItem(TypeSource item, TypeSource type) => string.Compare(item.Name, type.Name) == 0;

        protected abstract TypeSource GetDataContextTypeSource();

        protected virtual void ApplyActualContent(TypeSource type)
        {
            object content = GetActualContentResource(type);
            if (content is DataTemplate template)
            {
                content = template.LoadContent();
            }
            actualContent.Content = content;
        }

        protected virtual object GetActualContentResource(TypeSource type) => Application.Current.TryFindResource(type.Name + "ChangeableContent");

        protected void OnTypeComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                if (e.AddedItems[0] is TypeSource source)
                {
                    RequestChangeType(source);
                    ApplyActualContent(source);
                }
            }
        }

        protected virtual void RequestChangeType(TypeSource source)
        {
            if (ChangeTypeCommand != null)
            {
                ChangeTypeCommand.Execute(ChangeTypeCommandParameter);
            }
            else
            {
                ChangeTypeEventArgs changeTypeArgs = new ChangeTypeEventArgs(DataContext, source) {
                    RoutedEvent = TypeChangeEvent
                };
                RaiseEvent(changeTypeArgs);
            }
            GetBindingExpression(DataContextProperty)?.UpdateTarget();
        }

        protected virtual void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e) => OnTemplateApplied();
    }
}
