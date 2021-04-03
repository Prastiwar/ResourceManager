using System.Text;

namespace RPGDataEditor.Core
{
    public static class PrimitiveExtensions
    {
        public static string ToFirstLower(this string value) => char.ToLower(value[0]) + value[1..];

        public static T DeepClone<T>(this ICopiable value) => (T)value.DeepClone();

        public static string MakeFriendlyName(this string text, bool preserveAcronyms = false)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder(text.Length * 2);
            builder.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                {
                    char previousChar = text[i - 1];
                    if ((previousChar != ' ' && !char.IsUpper(previousChar)) ||
                        (preserveAcronyms && char.IsUpper(previousChar) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                    {
                        builder.Append(' ');
                    }
                }

                builder.Append(text[i]);
            }
            return builder.ToString();
        }
    }
}
