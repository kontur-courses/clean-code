using Markdown.Contracts;
using Markdown.Parsers;
using Markdown.Tokens;

namespace Markdown.HtmlParsers;

public class HtmlBoldTextParser : ITokenParser
{
    private readonly IReadOnlyDictionary<TokenType, ITokenParser> parsers;

    public HtmlBoldTextParser(IReadOnlyDictionary<TokenType, ITokenParser> parsers)
    {
        this.parsers = parsers;
    }

    public string Parse(Token token)
    {
        return ((BoldTextToken)token).ConcatChildren(parsers, "<strong>", "</strong>");
    }
}