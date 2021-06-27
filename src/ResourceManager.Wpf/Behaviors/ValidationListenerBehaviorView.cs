using FluentValidation.Results;
using ResourceManager.Wpf.Validation;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ResourceManager.Wpf.Behaviors
{
    public class ValidationListenerBehaviorView : ValidationListenerBehaviorBase<FrameworkElement>
    {
        private static readonly DependencyProperty IsValidProperty =
            DependencyProperty.RegisterAttached("IsValid", typeof(bool), typeof(CatchValidationBehaviorView));

        protected override void OnValidated(ValidationFailure error) => SetError(error == null ? "" : error.ErrorMessage);

        protected void SetError(string errorText)
        {
            bool isValid = string.IsNullOrEmpty(errorText);
            BindingExpressionBase binding = AssociatedObject.SetBinding(IsValidProperty, new Binding(".") { Source = isValid });
            if (!isValid)
            {
                ValidationError validationError = new ValidationError(new InvalidValidationRule(), binding) {
                    ErrorContent = errorText
                };
                System.Windows.Controls.Validation.MarkInvalid(binding, validationError);
                if (ValidableHook is INotifyPropertyChanged notifyPropertyChanged)
                {
                    notifyPropertyChanged.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
                }
            }
            else
            {
                System.Windows.Controls.Validation.ClearInvalid(binding);
            }
        }

        private void NotifyPropertyChanged_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            BindingExpression binding = AssociatedObject.GetBindingExpression(IsValidProperty);
            System.Windows.Controls.Validation.ClearInvalid(binding);
            if (sender is INotifyPropertyChanged notifyPropertyChanged)
            {
                notifyPropertyChanged.PropertyChanged -= NotifyPropertyChanged_PropertyChanged;
            }
        }
    }
}
