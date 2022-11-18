using Markdown.Contracts;
using Markdown.Parsers;
using Markdown.Tokens;

namespace Markdown.HtmlParsers;

public class HtmlHeaderParser : ITokenParser
{
    private readonly IReadOnlyDictionary<TokenType, ITokenParser> parsers;

    public HtmlHeaderParser(IReadOnlyDictionary<TokenType, ITokenParser> parsers)
    {
        this.parsers = parsers;
    }

    public string Parse(Token token)
    {
        return ((HeaderToken)token).ConcatChildren(parsers, "<h1>", "</h1>");
    }
}