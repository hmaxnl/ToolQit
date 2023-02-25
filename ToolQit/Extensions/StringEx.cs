namespace ToolQit.Extensions
{
    public static class StringEx
    {
        public static bool IsNullEmpty(this string value) => string.IsNullOrEmpty(value);
        public static bool IsNullWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);
        /// <summary>
        /// Check if the string is null, empty or is only white space.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns></returns>
        public static bool IsNullEmptyWhiteSpace(this string value) => value.IsNullEmpty() || value.IsNullWhiteSpace();
    }
}