using FluentValidation;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;

namespace RPGDataEditor.Core
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> ResourceLocation<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
            => ruleBuilder.Must(x => IsResourceLocation(x));

        public static IRuleBuilderOptions<T, TProperty> Url<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
            => ruleBuilder.Must(x => IsUrl(x));

        public static IRuleBuilderOptions<T, TProperty> Json<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
            => ruleBuilder.Must(x => IsJson(x.ToString()));

        public static bool IsJson(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
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

        public static bool IsResourceLocation(object obj)
        {
            if (obj == null || obj.ToString() == "")
            {
                return true;
            }
            string value = obj.ToString();
            string[] parts = value.Split(':', System.StringSplitOptions.RemoveEmptyEntries);
            return parts.Length == 2;
        }

        public static bool IsUrl(object obj)
        {
            if (obj == null)
            {
                return true;
            }
            string value = obj.ToString();
            return new UrlAttribute().IsValid(value);
        }
    }
}
