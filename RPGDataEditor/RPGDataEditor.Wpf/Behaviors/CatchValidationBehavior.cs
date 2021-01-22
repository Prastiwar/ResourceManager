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
                Validation.MarkInvalid(binding, validationError);
            }
            else
            {
                Validation.ClearInvalid(binding);
            }
        }
    }
}
