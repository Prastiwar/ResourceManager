using FluentValidation.Results;
using RPGDataEditor.Wpf.Validation;
using System.Windows.Controls;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Behaviors
{
    public class ValidationListenerBehavior : ValidationListenerBehaviorBase<TextBox>
    {
        protected override void OnValidated(ValidationFailure failure) => SetError(failure == null ? "" : failure.ErrorMessage);

        protected void SetError(string errorText)
        {
            BindingExpression binding = AssociatedObject.GetBindingExpression(TextBox.TextProperty);
            if (!string.IsNullOrEmpty(errorText))
            {
                ValidationError validationError = new ValidationError(new InvalidValidationRule(), binding) {
                    ErrorContent = errorText
                };
                System.Windows.Controls.Validation.MarkInvalid(binding, validationError);
            }
            else
            {
                System.Windows.Controls.Validation.ClearInvalid(binding);
            }
        }
    }
}
