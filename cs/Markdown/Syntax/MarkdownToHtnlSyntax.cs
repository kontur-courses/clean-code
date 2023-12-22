using Markdown.Token;
using Markdown.Tag;

namespace Markdown.Syntax;

public class MarkdownToHtnlSyntax : ISyntax
{
    private static readonly Dictionary<string, Func<int, IToken>> MarkdownToToken = new()
    {
        { "# ", pos => new HeaderToken(pos) }, { "_", pos => new ItalicToken(pos) },
        { "__", pos => new BoldToken(pos) }, { "\\", pos => new EscapeToken(pos) },
        { "\n", pos => new NewLineToken(pos) }, { "![", pos => new ImageToken(pos) },
        { "]", pos => new ImageToken(pos) }, { "(", pos => new ImageToken(pos) },
        { ")", pos => new ImageToken(pos) }
    };

    private static readonly Dictionary<string, IList<string>> TagNotWorkingWithinTags = new()
    {
        { "__", new List<string> { "_", "@" } }, { "_", new List<string> { "@" } },
        { "# ", new List<string> { "@" } }, { "\n", new List<string> { "@" } },
        { "\\", new List<string> { "@" } }
    };

    private static readonly Dictionary<string, List<string>> SupportedParameters = new()
    {
        { "@", new List<string> { "src", "alt" } }
    };

    private static readonly Dictionary<string, Func<ITag>> TokenSeparatorToHtmlTag = new()
    {
        { "# ", () => new HtmlTag("<h1>", "</h1>\n", true) },
        { "_", () => new HtmlTag("<em>", "</em>", true) },
        { "__", () => new HtmlTag("<strong>", "</strong>", true) },
        { "\\", () => new HtmlTag("", "", false) },
        { "@", () => new HtmlTag("<img>", "", true) }
    };

    public Type EscapeToken => typeof(EscapeToken);
    public string NewLineSeparator => new NewLineToken(0).Separator;

    public ITag ConvertTag(IToken token)
    {
        return TokenSeparatorToHtmlTag[token.Separator].Invoke();
    }

    public IReadOnlyDictionary<string, IList<string>> TagCannotBeInsideTags => TagNotWorkingWithinTags;
    public IReadOnlyDictionary<string, Func<int, IToken>> StringToToken => MarkdownToToken;

    public IList<string> GetSupportedTagParameters(string tagSeparator)
    {
        return SupportedParameters[tagSeparator];
    }
}