using Markdown.Token;
using Markdown.Tag;

namespace Markdown.Syntax;

public class MarkdownSyntax : ISyntax
{
    private readonly Dictionary<string, Func<int, IToken>> markdownToToken = new()
    {
        { "#", pos => new HeaderToken(pos) }, { "_", pos => new ItalicToken(pos) }, { "__", pos => new BoldToken(pos) },
        { "\\", pos => new EscapeToken(pos) }, { "\n", pos => new NewLineToken()}
    };

    private readonly Dictionary<string, IList<string>> tagNotWorkingWithinTags = new()
    {
        { new BoldToken(0).Separator, new List<string> { new ItalicToken(0).Separator } }
    };

    private readonly Dictionary<string, ITag> tokenSeparatorToHtml = new()
    {
        { "#", new HtmlTag("<h1>", "</h1>\n", true) }, { "_", new HtmlTag("<em>", "</em>", true) },
        { "__", new HtmlTag("<strong>", "</strong>", true) }, { "\\", new HtmlTag("", "", false) }
    };

    public Type EscapeToken => typeof(EscapeToken);
    public string NewLineSeparator => new NewLineToken().Separator;
    public ITag ConvertTag(IToken token)
    {
        return tokenSeparatorToHtml[token.Separator];
    }

    public IReadOnlyDictionary<string, IList<string>> UnsupportedTags => tagNotWorkingWithinTags;
    public IReadOnlyDictionary<string, Func<int, IToken>> StringToToken => markdownToToken;
}