namespace Core.Utils.Extensions
{
    public static class TextExtensions
    {
        private const string SucesiveDots = "...";

        public static string Ellipsis(this string text, int limit)
        {
            if (text.Length <= limit) return text;
            return text.Substring(0, limit) + SucesiveDots;
        }
    }
}