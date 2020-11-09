namespace Markdown
{
    public static class StringExtensions
    {
        public static bool IsChar(this string source, int index, char symbol) =>
            source.ContainsIndex(index) && source[index] == symbol;
        
        public static bool IsDigit(this string source, int index) =>
            source.ContainsIndex(index) && char.IsDigit(source[index]);
        
        public static bool IsLetter(this string source, int index) =>
            source.ContainsIndex(index) && char.IsLetter(source[index]);
        
        public static bool IsSpace(this string source, int index) =>
            source.ContainsIndex(index) && char.IsWhiteSpace(source[index]);
        
        public static bool IsTagStart(this string source, int index) =>
            source.ContainsIndex(index) && Tag.MdFirstChars.Contains(source[index]);

        private static bool ContainsIndex(this string source, int index) => index >= 0 && index < source.Length;
    }
}