using Markdown.Tokens;

namespace Markdown.Readers
{
    public interface IReader
    {
        (IToken, int) ReadToken(string text, int idx, ReadingOptions options);
    }
}