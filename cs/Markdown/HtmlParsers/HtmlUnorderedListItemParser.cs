using Markdown.Contracts;
using Markdown.Parsers;
using Markdown.Tokens;

namespace Markdown.HtmlParsers;

public class HtmlUnorderedListItemParser : ITokenParser
{
    private readonly IReadOnlyDictionary<TokenType, ITokenParser> parsers;

    public HtmlUnorderedListItemParser(IReadOnlyDictionary<TokenType, ITokenParser> parsers)
    {
        this.parsers = parsers;
    }

    public string Parse(Token token)
    {
        return ((UnorderedListItemToken)token).ConcatChildren(parsers, "<li>", "</li>");
    }
}