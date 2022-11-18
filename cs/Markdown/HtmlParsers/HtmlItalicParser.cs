using Markdown.Contracts;
using Markdown.Tokens;

namespace Markdown.HtmlParsers;

public class HtmlItalicParser : ITokenParser
{
    public string Parse(Token token)
    {
        var italicToken = (ItalicTextToken)token;
        return $"<em>{italicToken.Value}</em>";
    }
}