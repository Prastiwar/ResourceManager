using RPGDataEditor.Core;
using System.Globalization;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Validation
{
    public class ResourceLocationRule : ValidationRule
    {
        public bool AllowNull { get; set; } = true;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            bool isResourceLocation = AllowNull;
            if (value != null)
            {
                string val = value.ToString();
                isResourceLocation = ValidationExtensions.IsResourceLocation(val, AllowNull);
            }
            return new ValidationResult(isResourceLocation, "This is not valid resource location");
        }
    }
}
