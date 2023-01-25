namespace ToolQit.Extensions
{
    public static class StringEx
    {
        public static bool IsNullEmpty(this string value) => string.IsNullOrEmpty(value);
        public static bool IsNullWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);
    }
}