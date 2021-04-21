using FluentValidation;
using FluentValidation.Results;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Core.Validation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RPGDataEditor.Core
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> Url<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, bool allowNull = true)
            => ruleBuilder.Must(x => IsUrl(x, allowNull));

        public static IRuleBuilderOptions<T, TProperty> Json<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, bool allowNull = true)
            => ruleBuilder.Must(x => IsJson(x?.ToString(), allowNull));

        public static IRuleBuilderOptions<T, TProperty> NoException<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, Func<TProperty, Action> action)
        => ruleBuilder.Must(x => {
            try
            {
                action.Invoke(x).Invoke();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        });

        public static bool IsJson(string value, bool allowNull = true)
        {
            if (string.IsNullOrEmpty(value))
            {
                return allowNull;
            }
            value = value.Trim();
            bool isJsonObject = value.StartsWith("{") && value.EndsWith("}");
            bool isJsonArray = value.StartsWith("[") && value.EndsWith("]");
            if (isJsonObject || isJsonArray)
            {
                try
                {
                    _ = JToken.Parse(value);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        public static bool IsUrl(object obj, bool allowNull = true)
        {
            if (obj == null)
            {
                return allowNull;
            }
            string value = obj.ToString();
            return new System.ComponentModel.DataAnnotations.UrlAttribute().IsValid(value);
        }

        public static ValidationFailure Copy(this ValidationFailure failure) => new ValidationFailure(failure.PropertyName, failure.ErrorMessage, failure.AttemptedValue) {
            Severity = failure.Severity,
            CustomState = failure.CustomState,
            ErrorCode = failure.ErrorCode,
            FormattedMessageArguments = failure.FormattedMessageArguments,
            FormattedMessagePlaceholderValues = failure.FormattedMessagePlaceholderValues
        };


        /// <summary> Call NotifyValidate for each IValidable property (or each item in collection) with trimmed results recursively </summary>
        public static void NotifyValidateRecursive(this IValidable target, ValidationResult result)
        {
            HashSet<string> notifiedPropertyNames = new HashSet<string>();
            List<ValidationFailure> newFailures = new List<ValidationFailure>();
            foreach (ValidationFailure failure in result.Errors)
            {
                string[] propertyNames = failure.PropertyName.Split('.', StringSplitOptions.RemoveEmptyEntries);
                if (propertyNames.Length < 2)
                {
                    continue;
                }
                string targetPropertyName = GetNoArrayProperty(propertyNames[0]);
                if (!notifiedPropertyNames.Add(targetPropertyName))
                {
                    continue;
                }
                PropertyInfo property = target.GetType().GetProperty(targetPropertyName);
                bool cannotValidate = property.GetMethod == null || property.GetCustomAttribute<NotValidableAttribute>() != null;
                if (cannotValidate)
                {
                    continue;
                }
                object value = property.GetValue(target);
                if (value is IValidable validable)
                {
                    ValidationResult newResult = GetTargetPropertyResult(result, newFailures, targetPropertyName);
                    validable.NotifyValidate(newResult);
                }
                else if (value is IEnumerable enumerable && !(enumerable is IEnumerable<char>))
                {
                    ValidationResult collectionResults = GetTargetPropertyResult(result, newFailures, targetPropertyName);
                    List<object> enumerableItems = enumerable.Cast<object>().ToList();
                    foreach (ValidationFailure item in collectionResults.Errors)
                    {
                        int arrayEndIndex = item.PropertyName.IndexOf(']');
                        string itemIndexString = item.PropertyName.Substring(1, arrayEndIndex - 1);
                        int itemIndex = int.Parse(itemIndexString);
                        if (enumerableItems[itemIndex] is IValidable itemValidable)
                        {
                            string targetItemPropertyName = item.PropertyName.Substring(0, item.PropertyName.IndexOf('.'));
                            ValidationResult itemResults = GetTargetPropertyResult(collectionResults, newFailures, targetItemPropertyName);
                            itemValidable.NotifyValidate(itemResults);
                        }
                    }
                }
            }
        }

        private static string GetNoArrayProperty(string propertyName)
        {
            int startArrayIndex = propertyName.IndexOf('[');
            if (startArrayIndex > -1)
            {
                return propertyName.Substring(0, startArrayIndex);
            }
            return propertyName;
        }

        private static ValidationResult GetTargetPropertyResult(ValidationResult result, List<ValidationFailure> newFailures, string targetPropertyName)
        {
            newFailures.Clear();

            bool IsTargetFailure(ValidationFailure failure)
            {
                bool starts = failure.PropertyName.StartsWith(targetPropertyName);
                if (starts && failure.PropertyName.Length > targetPropertyName.Length)
                {
                    char nextChar = failure.PropertyName[targetPropertyName.Length];
                    return nextChar == '[' || nextChar == '.';
                }
                return starts;
            }

            IEnumerable<ValidationFailure> segmentFailures = result.Errors.Where(IsTargetFailure);
            foreach (ValidationFailure segmentFailure in segmentFailures)
            {
                bool isStartArray = segmentFailure.PropertyName[targetPropertyName.Length] == '[';
                int removeCount = isStartArray ? targetPropertyName.Length : targetPropertyName.Length + 1;
                string trimmedPropertyName = segmentFailure.PropertyName.Remove(0, removeCount);
                ValidationFailure newFailure = segmentFailure.Copy();
                newFailure.PropertyName = trimmedPropertyName;
                newFailures.Add(newFailure);
            }
            return new ValidationResult(newFailures);
        }
    }
}
