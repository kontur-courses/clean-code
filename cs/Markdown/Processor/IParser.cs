using Markdown.Token;

namespace Markdown.Processor;

public interface IParser
{
    IList<IToken> ParseTokens(string source);
}