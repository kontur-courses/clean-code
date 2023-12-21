using Markdown.Token;
using Markdown.Tag;

namespace Markdown.Syntax;

public class MarkdownToHtnlSyntax : ISyntax
{
    private readonly Dictionary<string, Func<int, IToken>> markdownToToken = new()
    {
        { "# ", pos => new HeaderToken(pos) }, { "_", pos => new ItalicToken(pos) },
        { "__", pos => new BoldToken(pos) }, { "\\", pos => new EscapeToken(pos) },
        { "\n", pos => new NewLineToken() }, { "@", pos => new ImageToken(pos) }
    };

    private readonly Dictionary<string, IList<string>> tagNotWorkingWithinTags = new()
    {
        { "__", new List<string> { "_", "@" } }, { "_", new List<string> { "@" } },
        { "# ", new List<string> { "@" } }, { "\n", new List<string> { "@" } },
        { "\\", new List<string> { "@" } }
    };

    private readonly Dictionary<string, ITag> tokenSeparatorToHtml = new()
    {
        { "# ", new HtmlTag("<h1>", "</h1>\n", true) },
        { "_", new HtmlTag("<em>", "</em>", true) },
        { "__", new HtmlTag("<strong>", "</strong>", true) },
        { "\\", new HtmlTag("", "", false) },
        { "@", new HtmlTag("<img>", "", true) }
    };

    private readonly Dictionary<string, List<string>> supportedParameters = new()
    {
        { "@", new List<string> { "src", "alt" } }
    };

    public Type EscapeToken => typeof(EscapeToken);
    public string NewLineSeparator => new NewLineToken().Separator;

    public ITag ConvertTag(IToken token)
    {
        return tokenSeparatorToHtml[token.Separator];
    }

    public IReadOnlyDictionary<string, IList<string>> TagCannotBeInsideTags => tagNotWorkingWithinTags;
    public IReadOnlyDictionary<string, Func<int, IToken>> StringToToken => markdownToToken;

    public IList<string> GetSupportedTagParameters(string tagSeparator)
    {
        return supportedParameters[tagSeparator];
    }
}