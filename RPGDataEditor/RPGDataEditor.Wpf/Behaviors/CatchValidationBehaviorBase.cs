using FluentValidation.Results;
using RPGDataEditor.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace RPGDataEditor.Wpf.Behaviors
{
    public abstract class CatchValidationBehaviorBase<T> : Behavior<T> where T : FrameworkElement
    {
        public static DependencyProperty PropertyNameProperty =
            DependencyProperty.Register(nameof(PropertyName), typeof(string), typeof(CatchValidationBehaviorBase<T>));

        public string PropertyName {
            get => (string)GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        public static DependencyProperty ValidableContextProperty =
            DependencyProperty.Register(nameof(ValidableContext), typeof(IValidable), typeof(CatchValidationBehaviorBase<T>), new PropertyMetadata(null, OnValidableContextChanged));

        public IValidable ValidableContext {
            get => (IValidable)GetValue(ValidableContextProperty);
            set => SetValue(ValidableContextProperty, value);
        }

        private static void OnValidableContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CatchValidationBehaviorBase<T> behavior = (CatchValidationBehaviorBase<T>)d;
            if (e.OldValue is IValidable oldValidable)
            {
                oldValidable.Validated -= behavior.Validable_OnValidated;
            }
            if (e.NewValue is IValidable newValidable)
            {
                newValidable.Validated -= behavior.Validable_OnValidated; // prevent event duplication
                newValidable.Validated += behavior.Validable_OnValidated;
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            bool hasBinding = BindingOperations.GetBindingExpression(this, ValidableContextProperty) != null;
            if (!hasBinding)
            {
                if (ValidableContext == null)
                {
                    ValidableContext = AssociatedObject.DataContext as IValidable;
                    AssociatedObject.DataContextChanged += AssociatedObject_DataContextChanged;
                }
                if (ValidableContext == null)
                {
                    ValidableContext = AssociatedObject.GetValue(ValidableHelper.ValidableObjectProperty) as IValidable;
                    DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(ValidableHelper.ValidableObjectProperty, AssociatedType);
                    dpd.AddValueChanged(AssociatedObject, OnValidableObjectChanged);
                }
            }
        }

        private void AssociatedObject_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) => OnValidableContextChanged(this, e);

        private void OnValidableObjectChanged(object sender, EventArgs e)
            => ValidableContext = AssociatedObject.GetValue(ValidableHelper.ValidableObjectProperty) as IValidable;

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (ValidableContext != null)
            {
                ValidableContext.Validated -= Validable_OnValidated;
            }
        }

        protected abstract void OnValidated(ValidationResult result);

        private void Validable_OnValidated(object sender, ValidationResult e) => OnValidated(e);

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

        protected string GetErrorMessage(string propertyName, IEnumerable<ValidationFailure> errors)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return errors.First().ErrorMessage;
            }
            else
            {
                foreach (ValidationFailure failure in errors)
                {
                    string propertyPath = failure.PropertyName;
                    if (propertyPath.CompareTo(propertyName) == 0)
                    {
                        return failure.ErrorMessage;
                    }
                }
            }
            return "";
        }
    }
}
