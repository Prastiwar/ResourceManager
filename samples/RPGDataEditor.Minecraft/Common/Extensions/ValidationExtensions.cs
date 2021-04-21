using FluentValidation;

namespace RPGDataEditor.Minecraft
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> ResourceLocation<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, bool allowNull = true)
            => ruleBuilder.Must(x => IsResourceLocation(x, allowNull));

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
    }
}
