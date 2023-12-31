using Markdown.Token;

namespace Markdown.Syntax;

public class MarkdownToTokenSyntax : IMarkupToTokenSyntax
{
    private static readonly Dictionary<string, Func<int, IToken>> MarkdownToToken = new()
    {
        { "# ", pos => new HeaderToken(pos) }, { "_", pos => new ItalicToken(pos) },
        { "__", pos => new BoldToken(pos) }, { "\\", pos => new EscapeToken(pos) },
        { "\n", pos => new NewLineToken(pos) }, { "![", pos => new ImageToken(pos) },
        { "]", pos => new ImageToken(pos) }, { "(", pos => new ImageToken(pos) },
        { ")", pos => new ImageToken(pos) }, { "\r\n", pos => new NewLineToken(pos, "\r\n") }
    };

    private static readonly Dictionary<string, IList<string>> TagNotWorkingWithinTags = new()
    {
        { "__", new List<string> { "_", "@" } }, { "_", new List<string> { "@" } },
        { "# ", new List<string> { "@" } }, { "\n", new List<string> { "@" } },
        { "\\", new List<string> { "@" } }
    };

    public Type EscapeToken => typeof(EscapeToken);
    public string[] NewLineSeparators => new[] { "\n", "\r\n" };

    public IReadOnlyDictionary<string, IList<string>> TagCannotBeInsideTags => TagNotWorkingWithinTags;
    public IReadOnlyDictionary<string, Func<int, IToken>> StringToToken => MarkdownToToken;
}