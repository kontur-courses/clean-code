using Markdown.Data;

namespace Markdown.TokenParser.TokenReaders
{
    public interface ITokenReader
    {
        TokenReaderResult ReadToken(string text, int startingPosition);
    }
}