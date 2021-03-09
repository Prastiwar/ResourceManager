using RPGDataEditor.Core;
using System.Globalization;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Validation
{
    public class JsonRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return ValidationResult.ValidResult;
            }
            string val = value.ToString();
            bool isJson = ValidationExtensions.IsJson(val);
            return new ValidationResult(isJson, "This is not valid json format");
        }
    }
}
