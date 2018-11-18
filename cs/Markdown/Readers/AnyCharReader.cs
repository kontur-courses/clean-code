using Markdown.Tokens;

namespace Markdown.Readers
{
    public class AnyCharReader : IReader
    {
        public (IToken, int) ReadToken(string text, int idx, ReadingOptions options)
        {
            return idx < text.Length ? (new TextToken(text[idx].ToString()), 1) : (null, 0);
        }
    }
}