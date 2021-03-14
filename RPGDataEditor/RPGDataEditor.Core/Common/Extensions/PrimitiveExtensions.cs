namespace RPGDataEditor.Core
{
    public static class PrimitiveExtensions
    {
        public static string ToFirstLower(this string value) => char.ToLower(value[0]) + value[1..];
    }
}
