using FluentValidation.Results;
using ResourceManager.Core.Validation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace ResourceManager.Wpf.Behaviors
{
    public abstract class ValidationListenerBehaviorBase<T> : Behavior<T> where T : FrameworkElement
    {
        public static DependencyProperty PropertyNameProperty =
            DependencyProperty.Register(nameof(PropertyName), typeof(string), typeof(ValidationListenerBehaviorBase<T>));
        public string PropertyName {
            get => (string)GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        public static DependencyProperty ValidableInstanceProperty =
            DependencyProperty.Register(nameof(ValidableInstance), typeof(object), typeof(ValidationListenerBehaviorBase<T>));
        public object ValidableInstance {
            get => GetValue(ValidableInstanceProperty);
            set => SetValue(ValidableInstanceProperty, value);
        }

        public static DependencyProperty ValidableHookProperty =
            DependencyProperty.Register(nameof(ValidableHook), typeof(IValidationHook), typeof(ValidationListenerBehaviorBase<T>), new PropertyMetadata(null, OnValidableHookChanged));
        public IValidationHook ValidableHook {
            get => (IValidationHook)GetValue(ValidableHookProperty);
            set => SetValue(ValidableHookProperty, value);
        }

        protected DependencyObject ValidableHookTarget { get; set; }

        private static void OnValidableHookChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ValidationListenerBehaviorBase<T> behavior = (ValidationListenerBehaviorBase<T>)d;
            if (e.OldValue is IValidationHook oldHook)
            {
                oldHook.Validated -= behavior.Validated;
            }
            if (e.NewValue is IValidationHook newHook)
            {
                newHook.Validated -= behavior.Validated; // prevent event duplication
                newHook.Validated += behavior.Validated;
                behavior.ValidableHook = newHook; // set context there since it's not always updated
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            if (ValidableHook == null && !this.HasBinding(ValidableHookProperty))
            {
                FindValidableHook();
            }
            if (ValidableInstance == null && !this.HasBinding(ValidableInstanceProperty))
            {
                FindValidableInstance();
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (ValidableHook != null)
            {
                ValidableHook.Validated -= Validated;
            }
        }

        protected virtual void FindValidableInstance()
        {
            if (!AssociatedObject.IsLoaded)
            {
                AssociatedObject.Loaded -= FindValidableInstanceOnLoaded; // prevent duplication
                AssociatedObject.Loaded += FindValidableInstanceOnLoaded;
                return;
            }
            AssociatedObject.Loaded -= FindValidableInstanceOnLoaded;
            FrameworkElement parent = AssociatedObject;
            while (ValidableInstance == null && parent != null)
            {
                if (parent.DataContext == null && !parent.HasBinding(FrameworkElement.DataContextProperty))
                {
                    parent = VisualTreeHelper.GetParent(parent) as FrameworkElement;
                    continue;
                }
                parent.DataContextChanged += UpdateValidableInstance;
                ValidableInstance = parent.DataContext;
                UpdateValidableInstance(parent, new DependencyPropertyChangedEventArgs(FrameworkElement.DataContextProperty, ValidableInstance, parent.DataContext));
                break;
            }
        }

        private void FindValidableInstanceOnLoaded(object sender, RoutedEventArgs e) => FindValidableInstance();

        private void UpdateValidableInstance(object sender, DependencyPropertyChangedEventArgs e)
        {
            BindingExpression dataContextExpression = (sender as FrameworkElement).GetBindingExpression(FrameworkElement.DataContextProperty);
            if (dataContextExpression?.ParentBinding.Converter != null)
            {
                ValidableInstance = dataContextExpression.ParentBinding.Converter.ConvertBack(e.NewValue, null, null, System.Globalization.CultureInfo.CurrentCulture);
            }
            else
            {
                ValidableInstance = e.NewValue;
            }
        }

        protected virtual void FindValidableHook()
        {
            if (!AssociatedObject.IsLoaded)
            {
                AssociatedObject.Loaded -= FindValidationHookOnLoaded; // prevent duplication
                AssociatedObject.Loaded += FindValidationHookOnLoaded;
                return;
            }
            AssociatedObject.Loaded -= FindValidationHookOnLoaded;
            DependencyObject parent = AssociatedObject;
            while (ValidableHookTarget == null && parent != null)
            {
                if (!ValidableHelper.HasHook(parent))
                {
                    parent = VisualTreeHelper.GetParent(parent);
                    continue;
                }
                DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(ValidableHelper.ValidableHookProperty, parent.GetType());
                dpd.AddValueChanged(parent, UpdateValidableHook);
                ValidableHookTarget = parent;
                UpdateValidableHook(this, null);
            }
        }

        private void FindValidationHookOnLoaded(object sender, RoutedEventArgs e) => FindValidableHook();

        private void UpdateValidableHook(object sender, EventArgs e) => ValidableHook = ValidableHelper.GetValidableHook(ValidableHookTarget);

        protected abstract void OnValidated(ValidationFailure error);

        public void Validated(object sender, ValidatedEventArgs e)
        {
            string catchPropertyName = GetCatchProperty();

            static bool AreEqual(string value, string value2) => string.Compare(value, value2) == 0;

            foreach (ValidationFailure error in e.Result.Errors)
            {
                if (AreEqual(error.PropertyName, catchPropertyName))
                {
                    OnValidated(error);
                    return;
                }
                bool hasLongerPath = error.PropertyName.Contains('.');
                if (!hasLongerPath)
                {
                    continue;
                }
                string[] errorPathParts = error.PropertyName.Split('.', StringSplitOptions.RemoveEmptyEntries);
                string lastPathPart = errorPathParts[errorPathParts.Length - 1];
                bool catchesLongerPath = catchPropertyName.Contains('.');
                if (catchesLongerPath)
                {
                    string[] catchPathParts = catchPropertyName.Split('.', StringSplitOptions.RemoveEmptyEntries);
                    bool isPathValid = IsPathValid(errorPathParts, catchPathParts);
                    if (!isPathValid)
                    {
                        continue;
                    }
                    catchPropertyName = errorPathParts[errorPathParts.Length - 1];
                }
                if (!AreEqual(lastPathPart, catchPropertyName))
                {
                    continue;
                }
                object currentInstance = e.Instance;
                foreach (string property in errorPathParts)
                {
                    string mutableProperty = property;
                    int arrayBraceStartIndex = mutableProperty.IndexOf('[');
                    int arrayIndex = -1;
                    if (arrayBraceStartIndex >= 0)
                    {
                        arrayIndex = int.Parse(mutableProperty.Substring(arrayBraceStartIndex).Remove(0, 1).Remove(1, 1));
                        mutableProperty = mutableProperty.Substring(0, arrayBraceStartIndex);
                    }
                    PropertyInfo info = currentInstance.GetType().GetProperty(mutableProperty, BindingFlags.Public | BindingFlags.Instance);
                    if (info?.GetMethod == null)
                    {
                        break;
                    }
                    object value = info.GetValue(currentInstance);
                    if (value == null)
                    {
                        break;
                    }
                    if (value is Array array)
                    {
                        currentInstance = array.GetValue(arrayIndex);
                    }
                    else if (value is IEnumerable enumerable)
                    {
                        int index = 0;
                        foreach (object obj in enumerable)
                        {
                            if (index == arrayIndex)
                            {
                                currentInstance = obj;
                                break;
                            }
                            index++;
                        }
                    }
                    else
                    {
                        currentInstance = value;
                    }
                    if (currentInstance == ValidableInstance)
                    {
                        OnValidated(error);
                        return;
                    }
                }
            }
            OnValidated(null);
        }

        private bool IsPathValid(string[] errorPathParts, string[] catchPathParts)
        {
            bool isPathValid = true;
            for (int i = 0; i < errorPathParts.Length; i++)
            {
                int index = Array.IndexOf(errorPathParts, catchPathParts[i], i);
                if (index < 0)
                {
                    isPathValid = false;
                    break;
                }
            }
            return isPathValid;
        }

        protected string GetCatchProperty()
        {
            string catchProperty = PropertyName;
            if (string.IsNullOrEmpty(PropertyName))
            {
                Binding binding = BindingOperations.GetBinding(AssociatedObject, System.Windows.Controls.TextBox.TextProperty);
                catchProperty = binding?.Path.Path;
            }
            string pathFormat = ValidableHelper.GetValidablePathFormat(AssociatedObject);
            if (!string.IsNullOrEmpty(pathFormat))
            {
                IList<object> values = ValidableHelper.GetValidablePathValues(AssociatedObject);
                if (values == null)
                {
                    catchProperty = pathFormat + "." + catchProperty;
                }
                else
                {
                    string path = string.Format(pathFormat, values.ToArray());
                    catchProperty = path + "." + catchProperty;
                }
            }
            return catchProperty;
        }
    }
}
