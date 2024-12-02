using Markdown.Tokens;

namespace Markdown.TokenParsers.TokenReaders;

public interface ITokenReader
{
    public IEnumerable<Token> ReadTokens(string text);
}