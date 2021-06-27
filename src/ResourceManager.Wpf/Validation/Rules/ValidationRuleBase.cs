using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace ResourceManager.Wpf.Validation
{
    public abstract class ValidationRuleBase : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is BindingExpression exp)
            {
                object resolvedValue = null;
                try
                {
                    resolvedValue = exp.GetResolvedValue();
                }
                catch (System.Exception)
                {
                    return new ValidationResult(false, "Internal error: Couldn't get value from binding expression");
                }
                return ValidateValue(resolvedValue, cultureInfo);
            }
            else
            {
                return ValidateValue(value, cultureInfo);
            }
        }

        protected abstract ValidationResult ValidateValue(object value, CultureInfo cultureInfo);
    }
}
