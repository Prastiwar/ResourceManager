using FluentValidation.Results;
using RPGDataEditor.Core.Validation;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace RPGDataEditor.Wpf.Behaviors
{
    public abstract class CatchValidationBehaviorBase<T> : Behavior<T> where T : FrameworkElement
    {
        public string PropertyName { get; set; }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.DataContextChanged += Bindable_DataContextChanged;
            if (AssociatedObject.DataContext is IValidable validable)
            {
                validable.Validated += Validable_OnValidated;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.DataContextChanged -= Bindable_DataContextChanged;
            if (AssociatedObject.DataContext is IValidable validable)
            {
                validable.Validated -= Validable_OnValidated;
            }
        }

        private void Bindable_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (AssociatedObject.DataContext is IValidable validable)
            {
                validable.Validated -= Validable_OnValidated; // prevent event duplication
                validable.Validated += Validable_OnValidated;
            }
        }

        protected abstract void OnValidated(ValidationResult result);

        private void Validable_OnValidated(object sender, ValidationResult e)
        {
            // Detach this event when binding context was changed
            if (AssociatedObject.DataContext != sender)
            {
                ((IValidable)sender).Validated -= Validable_OnValidated;
                return;
            }
            OnValidated(e);
        }

        protected string GetCatchProperty()
        {
            string catchProperty = PropertyName;
            if (string.IsNullOrEmpty(PropertyName))
            {
                Binding binding = BindingOperations.GetBinding(AssociatedObject, System.Windows.Controls.TextBox.TextProperty);
                catchProperty = binding?.Path.Path;
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
                    string latestName = propertyPath.Split('.', System.StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
                    string latestPropertyName = propertyName.Split('.', System.StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
                    if (latestName.CompareTo(latestPropertyName) == 0)
                    {
                        return failure.ErrorMessage;
                    }
                }
            }
            return "";
        }
    }
}
