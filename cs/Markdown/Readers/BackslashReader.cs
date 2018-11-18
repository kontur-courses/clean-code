using Markdown.Tokens;

namespace Markdown.Readers
{
    public class BackslashReader : IReader
    {
        public (IToken, int) ReadToken(string text, int idx, ReadingOptions options)
        {
            var nextSymbolIdx = idx + 1;
            if (nextSymbolIdx < text.Length + 1 && text[idx] == '\\')
                return (new TextToken(text[nextSymbolIdx].ToString()), 2);
            return (null, 0);
        }
    }
}