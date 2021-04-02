using FluentValidation;
using FluentValidation.Results;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;

namespace RPGDataEditor.Core
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> ResourceLocation<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, bool allowNull = true)
            => ruleBuilder.Must(x => IsResourceLocation(x, allowNull));

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

        public static bool IsResourceLocation(object obj, bool allowNull = true)
        {
            if (obj == null || obj.ToString() == "")
            {
                return allowNull;
            }
            string value = obj.ToString();
            string[] parts = value.Split(':', System.StringSplitOptions.RemoveEmptyEntries);
            return parts.Length == 2;
        }

        public static bool IsUrl(object obj, bool allowNull = true)
        {
            if (obj == null)
            {
                return allowNull;
            }
            string value = obj.ToString();
            return new UrlAttribute().IsValid(value);
        }

        public static ValidationFailure Copy(this ValidationFailure failure) => new ValidationFailure(failure.PropertyName, failure.ErrorMessage, failure.AttemptedValue) {
            Severity = failure.Severity,
            CustomState = failure.CustomState,
            ErrorCode = failure.ErrorCode,
            FormattedMessageArguments = failure.FormattedMessageArguments,
            FormattedMessagePlaceholderValues = failure.FormattedMessagePlaceholderValues
        };
    }
}
