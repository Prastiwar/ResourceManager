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
                validable.OnValidated += Validable_OnValidated;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.DataContextChanged -= Bindable_DataContextChanged;
            if (AssociatedObject.DataContext is IValidable validable)
            {
                validable.OnValidated -= Validable_OnValidated;
            }
        }

        private void Bindable_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (AssociatedObject.DataContext is IValidable validable)
            {
                validable.OnValidated -= Validable_OnValidated; // prevent event duplication
                validable.OnValidated += Validable_OnValidated;
            }
        }

        protected abstract void OnValidated(ValidationResult result);

        private void Validable_OnValidated(object sender, ValidationResult e)
        {
            // Detach this event when binding context was changed
            if (AssociatedObject.DataContext != sender)
            {
                ((IValidable)sender).OnValidated -= Validable_OnValidated;
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
                    if (failure.PropertyName == propertyName)
                    {
                        return failure.ErrorMessage;
                    }
                }
            }
            return "";
        }
    }
}
