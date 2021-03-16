using RPGDataEditor.Core;
using RPGDataEditor.Core.Validation;
using System.Globalization;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Validation
{
    public class ResourceLocationRule : ValidationRuleBase
    {
        public bool AllowNull { get; set; } = true;

        protected override ValidationResult ValidateValue(object value, CultureInfo cultureInfo)
        {
            bool isResourceLocation = AllowNull;
            if (value != null)
            {
                string val = value.ToString();
                isResourceLocation = ValidationExtensions.IsResourceLocation(val, AllowNull);
            }
            return new ValidationResult(isResourceLocation, CustomMessages.ResourceLocation);
        }
    }
}
