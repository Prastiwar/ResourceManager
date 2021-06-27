using ResourceManager.Wpf.Converters;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace ResourceManager.Wpf.Behaviors
{
    public class SetBindingBehavior : Behavior<DependencyObject>
    {
        public static readonly DependencyProperty TargetPropertyProperty =
            DependencyProperty.Register("TargetProperty", typeof(DependencyProperty), typeof(SetBindingBehavior), new PropertyMetadata(null, OnTargetPropertyChanged));
        public DependencyProperty TargetProperty {
            get => (DependencyProperty)GetValue(TargetPropertyProperty);
            set => SetValue(TargetPropertyProperty, value);
        }

        public static readonly DependencyProperty PathProperty =
            DependencyProperty.Register("Path", typeof(string), typeof(SetBindingBehavior), new PropertyMetadata(".", UpdateBinding));
        public string Path {
            get => (string)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }

        public static readonly DependencyProperty ConverterProperty =
            DependencyProperty.Register("Converter", typeof(IValueConverter), typeof(SetBindingBehavior), new PropertyMetadata(StaticValueConverter.Create(x => x.ToString(), null), UpdateBinding));
        public IValueConverter Converter {
            get => (IValueConverter)GetValue(ConverterProperty);
            set => SetValue(ConverterProperty, value);
        }

        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register("Mode", typeof(BindingMode), typeof(SetBindingBehavior), new PropertyMetadata(BindingMode.Default, UpdateBinding));
        public BindingMode Mode {
            get => (BindingMode)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }

        public static readonly DependencyProperty NotifyOnSourceUpdatedProperty =
            DependencyProperty.Register("NotifyOnSourceUpdated", typeof(bool), typeof(SetBindingBehavior), new PropertyMetadata(false, UpdateBinding));
        public bool NotifyOnSourceUpdated {
            get => (bool)GetValue(NotifyOnSourceUpdatedProperty);
            set => SetValue(NotifyOnSourceUpdatedProperty, value);
        }

        public static readonly DependencyProperty NotifyOnTargetUpdatedProperty =
            DependencyProperty.Register("NotifyOnTargetUpdated", typeof(bool), typeof(SetBindingBehavior), new PropertyMetadata(false, UpdateBinding));
        public bool NotifyOnTargetUpdated {
            get => (bool)GetValue(NotifyOnTargetUpdatedProperty);
            set => SetValue(NotifyOnTargetUpdatedProperty, value);
        }

        public static readonly DependencyProperty NotifyOnValidationErrorProperty =
            DependencyProperty.Register("NotifyOnValidationError", typeof(bool), typeof(SetBindingBehavior), new PropertyMetadata(false, UpdateBinding));
        public bool NotifyOnValidationError {
            get => (bool)GetValue(NotifyOnValidationErrorProperty);
            set => SetValue(NotifyOnValidationErrorProperty, value);
        }

        public static readonly DependencyProperty RelativeSourceProperty =
            DependencyProperty.Register("RelativeSource", typeof(RelativeSource), typeof(SetBindingBehavior), new PropertyMetadata(null, UpdateBinding));
        public RelativeSource RelativeSource {
            get => (RelativeSource)GetValue(RelativeSourceProperty);
            set => SetValue(RelativeSourceProperty, value);
        }

        public static readonly DependencyProperty UpdateSourceTriggerProperty =
            DependencyProperty.Register("UpdateSourceTrigger", typeof(UpdateSourceTrigger), typeof(SetBindingBehavior), new PropertyMetadata(UpdateSourceTrigger.Default, UpdateBinding));
        public UpdateSourceTrigger UpdateSourceTrigger {
            get => (UpdateSourceTrigger)GetValue(UpdateSourceTriggerProperty);
            set => SetValue(UpdateSourceTriggerProperty, value);
        }

        public static readonly DependencyProperty IsAsyncProperty =
            DependencyProperty.Register("IsAsync", typeof(bool), typeof(SetBindingBehavior), new PropertyMetadata(false, UpdateBinding));
        public bool IsAsync {
            get => (bool)GetValue(IsAsyncProperty);
            set => SetValue(IsAsyncProperty, value);
        }

        public static readonly DependencyProperty ValidatesOnDataErrorsProperty =
            DependencyProperty.Register("ValidatesOnDataErrors", typeof(bool), typeof(SetBindingBehavior), new PropertyMetadata(false, UpdateBinding));
        public bool ValidatesOnDataErrors {
            get => (bool)GetValue(ValidatesOnDataErrorsProperty);
            set => SetValue(ValidatesOnDataErrorsProperty, value);
        }

        public static readonly DependencyProperty ValidatesOnExceptionsProperty =
            DependencyProperty.Register("ValidatesOnExceptions", typeof(bool), typeof(SetBindingBehavior), new PropertyMetadata(false, UpdateBinding));
        public bool ValidatesOnExceptions {
            get => (bool)GetValue(ValidatesOnExceptionsProperty);
            set => SetValue(ValidatesOnExceptionsProperty, value);
        }

        public static readonly DependencyProperty ValidatesOnNotifyDataErrorsProperty =
            DependencyProperty.Register("ValidatesOnNotifyDataErrors", typeof(bool), typeof(SetBindingBehavior), new PropertyMetadata(true, UpdateBinding));
        public bool ValidatesOnNotifyDataErrors {
            get => (bool)GetValue(ValidatesOnNotifyDataErrorsProperty);
            set => SetValue(ValidatesOnNotifyDataErrorsProperty, value);
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(object), typeof(SetBindingBehavior), new PropertyMetadata(null, UpdateBinding));
        public object Source {
            get => GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static readonly DependencyProperty ElementNameProperty =
            DependencyProperty.Register("ElementName", typeof(string), typeof(SetBindingBehavior), new PropertyMetadata(null, UpdateBinding));
        public string ElementName {
            get => (string)GetValue(ElementNameProperty);
            set => SetValue(ElementNameProperty, value);
        }

        public static readonly DependencyProperty StringFormatProperty =
            DependencyProperty.Register("StringFormat", typeof(string), typeof(SetBindingBehavior), new PropertyMetadata(null, UpdateBinding));
        public string StringFormat {
            get => (string)GetValue(StringFormatProperty);
            set => SetValue(StringFormatProperty, value);
        }

        public static readonly DependencyProperty BindingGroupNameProperty =
            DependencyProperty.Register("BindingGroupName", typeof(string), typeof(SetBindingBehavior), new PropertyMetadata("", UpdateBinding));
        public string BindingGroupName {
            get => (string)GetValue(BindingGroupNameProperty);
            set => SetValue(BindingGroupNameProperty, value);
        }

        public static readonly DependencyProperty ConverterCultureProperty =
            DependencyProperty.Register("ConverterCulture", typeof(CultureInfo), typeof(SetBindingBehavior), new PropertyMetadata(null, UpdateBinding));
        public CultureInfo ConverterCulture {
            get => (CultureInfo)GetValue(ConverterCultureProperty);
            set => SetValue(ConverterCultureProperty, value);
        }

        public static readonly DependencyProperty BindsDirectlyToSourceProperty =
            DependencyProperty.Register("BindsDirectlyToSource", typeof(bool), typeof(SetBindingBehavior), new PropertyMetadata(false, UpdateBinding));
        public bool BindsDirectlyToSource {
            get => (bool)GetValue(BindsDirectlyToSourceProperty);
            set => SetValue(BindsDirectlyToSourceProperty, value);
        }

        public static readonly DependencyProperty AsyncStateProperty =
            DependencyProperty.Register("AsyncState", typeof(object), typeof(SetBindingBehavior), new PropertyMetadata(null, UpdateBinding));
        public object AsyncState {
            get => GetValue(AsyncStateProperty);
            set => SetValue(AsyncStateProperty, value);
        }

        public static readonly DependencyProperty TargetNullValueProperty =
            DependencyProperty.Register("TargetNullValue", typeof(object), typeof(SetBindingBehavior), new PropertyMetadata(null, UpdateBinding));
        public object TargetNullValue {
            get => GetValue(TargetNullValueProperty);
            set => SetValue(TargetNullValueProperty, value);
        }

        public static readonly DependencyProperty ConverterParameterProperty =
            DependencyProperty.Register("ConverterParameter", typeof(object), typeof(SetBindingBehavior), new PropertyMetadata(null, UpdateBinding));
        public object ConverterParameter {
            get => GetValue(ConverterParameterProperty);
            set => SetValue(ConverterParameterProperty, value);
        }

        public static readonly DependencyProperty FallbackValueProperty =
            DependencyProperty.Register("FallbackValue", typeof(object), typeof(SetBindingBehavior), new PropertyMetadata(null, UpdateBinding));
        public object FallbackValue {
            get => GetValue(FallbackValueProperty);
            set => SetValue(FallbackValueProperty, value);
        }

        public static readonly DependencyProperty XPathProperty =
            DependencyProperty.Register("XPath", typeof(string), typeof(SetBindingBehavior), new PropertyMetadata(null, UpdateBinding));
        public string XPath {
            get => (string)GetValue(XPathProperty);
            set => SetValue(XPathProperty, value);
        }

        public static readonly DependencyProperty DelayProperty =
            DependencyProperty.Register("Delay", typeof(int), typeof(SetBindingBehavior), new PropertyMetadata(0, UpdateBinding));
        public int Delay {
            get => (int)GetValue(DelayProperty);
            set => SetValue(DelayProperty, value);
        }

        private static void UpdateBinding(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as SetBindingBehavior).UpdateBinding();

        private static void OnTargetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetBindingBehavior behavior = d as SetBindingBehavior;
            if (e.OldValue is DependencyProperty oldProperty)
            {
                behavior.RemoveBinding(oldProperty);
            }
            behavior.UpdateBinding();
        }

        protected void RemoveBinding(DependencyProperty oldProperty)
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.ClearValue(oldProperty);
            }
        }

        protected void UpdateBinding()
        {
            if (TargetProperty != null && AssociatedObject != null)
            {
                Binding binding = new Binding(Path) {
                    Converter = Converter,
                    AsyncState = AsyncState,
                    IsAsync = IsAsync,
                    BindingGroupName = BindingGroupName,
                    BindsDirectlyToSource = BindsDirectlyToSource,
                    ConverterCulture = ConverterCulture,
                    ConverterParameter = ConverterParameter,
                    Delay = Delay,
                    FallbackValue = FallbackValue,
                    Mode = Mode,
                    NotifyOnSourceUpdated = NotifyOnSourceUpdated,
                    NotifyOnTargetUpdated = NotifyOnTargetUpdated,
                    NotifyOnValidationError = NotifyOnValidationError,
                    Path = new PropertyPath(Path),
                    RelativeSource = RelativeSource,
                    StringFormat = StringFormat,
                    TargetNullValue = TargetNullValue,
                    UpdateSourceTrigger = UpdateSourceTrigger,
                    ValidatesOnDataErrors = ValidatesOnDataErrors,
                    ValidatesOnExceptions = ValidatesOnExceptions,
                    ValidatesOnNotifyDataErrors = ValidatesOnNotifyDataErrors,
                    XPath = XPath,
                };
                if (Source != null)
                {
                    binding.Source = Source;
                }
                else if (!string.IsNullOrEmpty(ElementName))
                {
                    binding.ElementName = ElementName;
                }
                AssociatedObject.SetBinding(TargetProperty, binding);
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            UpdateBinding();
        }
    }
}
