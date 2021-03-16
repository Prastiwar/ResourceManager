using RPGDataEditor.Core.Validation;
using System.Globalization;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Validation
{
    public class NotEmptyRule : ValidationRuleBase
    {
        protected override ValidationResult ValidateValue(object value, CultureInfo cultureInfo)
        {
            bool isNull = value == null;
            if (!isNull)
            {
                string val = value.ToString();
                isNull = string.IsNullOrEmpty(val);
            }
            return new ValidationResult(isNull, CustomMessages.Empty);
        }
    }
}
