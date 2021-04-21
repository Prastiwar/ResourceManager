using RPGDataEditor.Core;
using RPGDataEditor.Core.Validation;
using System.Globalization;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Validation
{
    public class JsonRule : ValidationRuleBase
    {
        public bool AllowNull { get; set; } = true;

        protected override ValidationResult ValidateValue(object value, CultureInfo cultureInfo)
        {
            bool isJson = AllowNull;
            if (value != null)
            {
                string val = value.ToString();
                isJson = ValidationExtensions.IsJson(val, AllowNull);
            }
            return new ValidationResult(isJson, CustomMessages.Json);
        }
    }
}
