using System.Globalization;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Validation
{
    internal class InvalidValidationRule : ValidationRule
    {
        internal InvalidValidationRule(string errorText = null) => ErrorText = errorText;

        public string ErrorText { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo) => new ValidationResult(false, ErrorText);
    }
}
