using Markdown.Contracts;
using Markdown.Tokens;

namespace Markdown.HtmlParsers;

public class HtmlTextParser : ITokenParser
{
    public string Parse(Token token)
    {
        return ((TextToken)token).Value;
    }
}