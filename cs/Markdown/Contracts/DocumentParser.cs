using Markdown.Parsers;
using Markdown.Tokens;

namespace Markdown.Contracts;

public class DocumentParser : ITokenParser
{
    private readonly IReadOnlyDictionary<TokenType, ITokenParser> parsers;

    public DocumentParser(IReadOnlyDictionary<TokenType, ITokenParser> parsers)
    {
        this.parsers = parsers;
    }

    public string Parse(Token token)
    {
        return ((DocumentToken)token).ConcatChildren(parsers);
    }
}