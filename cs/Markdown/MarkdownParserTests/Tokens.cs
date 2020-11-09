using MarkdownParser.Infrastructure.Abstract;
using MarkdownParser.Infrastructure.Impl.Bold;
using MarkdownParser.Infrastructure.Impl.Italic;

namespace MarkdownParserTests
{
    public static class Tokens
    {
        public static TokenBold Bold(int position) => new TokenBold(position, 2, "__");
        public static TokenItalic Italic(int position) => new TokenItalic(position, 1, "_");
        public static TokenText Text(int position, string text) => new TokenText(position, text.Length, text);
    }
}