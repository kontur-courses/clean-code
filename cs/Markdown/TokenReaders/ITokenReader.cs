using Markdown.Tokens;

namespace Markdown.TokenReaders
{
    public interface ITokenReader
    {
        TextToken TyrGetToken(string text, int start, int end);
    }
}