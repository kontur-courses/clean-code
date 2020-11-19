using Markdown.Tokens;

namespace Markdown.TokenReaders
{
    public class HeaderTokenReader : ITokenReader
    {
        public TextToken TyrGetToken(string text, int start, int end)
        {
            return !CanCreateToken(text, start, end) ? null : new HeaderTextToken(text);
        }

        private static bool CanCreateToken(string text, int start, int end)
        {
            return end == start && text[start] == '#'
                                && end + 1 != text.Length && text[end + 1] != '\\';
        }
    }
}