using Markdown.Tag;
using Markdown.Token;

namespace Markdown.Syntax;

public class TokenToHtmlSyntax : ITokenToMarkupSyntax
{
    private static readonly Dictionary<string, Func<ITag>> TokenSeparatorToHtmlTag = new()
    {
        { "# ", () => new HtmlTag("<h1>", "</h1>\n", true) },
        { "_", () => new HtmlTag("<em>", "</em>", true) },
        { "__", () => new HtmlTag("<strong>", "</strong>", true) },
        { "\\", () => new HtmlTag("", "", false) },
        { "@", () => new HtmlTag("<img>", "", false) },
        { "\n", () => new HtmlTag("\n", "", false) },
        { "\r\n", () => new HtmlTag("\r\n", "", false) }
    };

    private static readonly Dictionary<string, List<string>> SupportedParameters = new()
    {
        { "@", new List<string> { "src", "alt" } }
    };

    public ITag ConvertTag(IToken token)
    {
        return TokenSeparatorToHtmlTag[token.Separator].Invoke();
    }

    public IList<string> GetSupportedTagParameters(string tagSeparator)
    {
        return SupportedParameters[tagSeparator];
    }
}