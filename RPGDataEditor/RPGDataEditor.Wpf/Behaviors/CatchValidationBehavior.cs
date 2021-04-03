using RPGDataEditor.Wpf.Validation;
using System.Windows.Controls;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Behaviors
{
    public class CatchValidationBehavior : CatchValidationBehaviorBase<TextBox>
    {
        protected override void OnValidated(FluentValidation.Results.ValidationResult result)
        {
            if (result.IsValid)
            {
                SetError("");
            }
            else
            {
                string catchProperty = GetCatchProperty();
                string errorMessage = GetErrorMessage(catchProperty, result.Errors);
                SetError(errorMessage);
            }
        }

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
