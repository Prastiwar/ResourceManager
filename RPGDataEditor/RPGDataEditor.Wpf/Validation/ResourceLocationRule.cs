using RPGDataEditor.Core;
using System.Globalization;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Validation
{
    public class ResourceLocationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return ValidationResult.ValidResult;
            }
            string val = value.ToString();
            bool isResourceLocation = ValidationExtensions.IsResourceLocation(val);
            return new ValidationResult(isResourceLocation, "This is not valid resource location");
        }
    }
}
