using Markdown.Token;

namespace Markdown.Parser;

public interface IParser
{
    IList<IToken> ParseTokens(string source);
}