using RPGDataEditor.Core;
using System.Globalization;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Validation
{
    public class JsonRule : ValidationRule
    {
        public bool AllowNull { get; set; } = true;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            bool isJson = AllowNull;
            if (value != null)
            {
                string val = value.ToString();
                isJson = ValidationExtensions.IsJson(val, AllowNull);
            }
            return new ValidationResult(isJson, "This is not valid json format");
        }
    }
}
