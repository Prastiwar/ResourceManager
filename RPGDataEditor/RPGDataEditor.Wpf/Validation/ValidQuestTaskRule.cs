using RPGDataEditor.Core;
using RPGDataEditor.Core.Models;
using System.Globalization;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Validation
{
    public class ValidQuestTaskRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return ValidationResult.ValidResult;
            }
            if (value is QuestTask task)
            {
                switch (task)
                {
                    case KillQuestTask killTask:
                        return ValidateResourceLocation(killTask.Kill);
                    case RightItemInteractQuestTask itemTask:
                        ValidationResult result = ValidateResourceLocation(itemTask.Item);
                        if (result.IsValid)
                        {
                            result = new ValidationResult(ValidationExtensions.IsJson(itemTask.Nbt), "This is not valid json format");
                        }
                        return result;
                    default:
                        break;
                }
                return ValidationResult.ValidResult;
            }
            return new ValidationResult(false, "This is not Quest Task type");
        }

        private ValidationResult ValidateResourceLocation(string value)
            => new ValidationResult(!string.IsNullOrEmpty(value) && ValidationExtensions.IsResourceLocation(value, false), "This is not valid resource location");
    }
}
