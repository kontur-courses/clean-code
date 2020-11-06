namespace Markdown
{
    public static class SafetyCharChecker
    {
        public static bool IsDigit(this string source, int index) =>
            index >= 0 && index < source.Length && char.IsDigit(source[index]);
        
        public static bool IsLetter(this string source, int index) =>
            index >= 0 && index < source.Length && char.IsLetter(source[index]);
        
        public static bool IsSpace(this string source, int index) =>
            index >= 0 && index < source.Length && char.IsWhiteSpace(source[index]);
    }
}