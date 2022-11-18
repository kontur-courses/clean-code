using Markdown.Contracts;
using Markdown.Parsers;
using Markdown.Tokens;

namespace Markdown.HtmlParsers;

public class HtmlParagraphParser : ITokenParser
{
    private readonly IReadOnlyDictionary<TokenType, ITokenParser> parsers;

    public HtmlParagraphParser(IReadOnlyDictionary<TokenType, ITokenParser> parsers)
    {
        this.parsers = parsers;
    }

    public string Parse(Token token)
    {
        return ((ParagraphToken)token).ConcatChildren(parsers, "<p>", "</p>");
    }
}