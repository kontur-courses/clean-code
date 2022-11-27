using Markdown.Contracts;
using Markdown.Parsers;
using Markdown.Tokens;

namespace Markdown.HtmlParsers;

public class HtmlUnorderedListParser : ITokenParser
{
    private readonly IReadOnlyDictionary<TokenType, ITokenParser> parsers;

    public HtmlUnorderedListParser(IReadOnlyDictionary<TokenType, ITokenParser> parsers)
    {
        this.parsers = parsers;
    }

    public string Parse(Token token)
    {
        return ((UnorderedListToken)token).ConcatChildren(parsers, "<ul>", "</ul>");
    }
}