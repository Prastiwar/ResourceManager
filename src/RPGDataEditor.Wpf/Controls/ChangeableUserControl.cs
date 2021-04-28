using System;
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
            public ChangeTypeEventArgs(object item, Type targetType)
            {
                Item = item;
                TargetType = targetType;
            }

            public object Item { get; }
            public Type TargetType { get; }
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
            Type type = GetDataContextItemType();
            if (type != null)
            {
                ApplyActualContent(type);
                typeComboBox.SelectedItem = typeComboBox.Items.Cast<object>().FirstOrDefault(item => CompareItem(item, type));
            }
            typeComboBox.SelectionChanged += OnTypeComboBoxSelectionChanged;
        }

        protected virtual bool CompareItem(object item, Type type)
        {
            Type itemType = item switch {
                ComboBoxItem boxItem => boxItem.Content as Type,
                TypeSource source => source.Type,
                _ => item as Type,
            };
            return itemType == type;
        }

        protected abstract Type GetDataContextItemType();

        protected virtual void ApplyActualContent(Type type)
        {
            object content = GetActualContentResource(type);
            if (content is DataTemplate template)
            {
                content = template.LoadContent();
            }
            actualContent.Content = content;
        }

        protected virtual object GetActualContentResource(Type type) => Application.Current.TryFindResource(type.Name + "ChangeableContent");

        protected void OnTypeComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                Type type = e.AddedItems[0] switch {
                    ComboBoxItem selected => selected.Content as Type,
                    TypeSource source => source.Type,
                    _ => e.AddedItems[0] as Type,
                };
                RequestChangeType(type);
                ApplyActualContent(type);
            }
        }

        protected virtual void RequestChangeType(Type type)
        {
            if (ChangeTypeCommand != null)
            {
                ChangeTypeCommand.Execute(ChangeTypeCommandParameter);
            }
            else
            {
                ChangeTypeEventArgs changeTypeArgs = new ChangeTypeEventArgs(DataContext, type) {
                    RoutedEvent = TypeChangeEvent
                };
                RaiseEvent(changeTypeArgs);
            }
        }

        protected virtual void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e) => OnTemplateApplied();
    }
}
