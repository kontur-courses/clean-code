using Markdown.Token;
using Markdown.Tag;

namespace Markdown.Syntax;

public class MarkdownSyntax : ISyntax
{
    private readonly Dictionary<string, Func<int, IToken>> markdownToToken = new Dictionary<string, Func<int, IToken>>
    {
        { "#", pos => new HeaderToken(pos) }, { "_", pos => new ItalicToken(pos) }, { "__", pos => new BoldToken(pos) },
        { "\\", pos => new EscapeToken(pos) }
    };

    private readonly Dictionary<string, IList<string>> tagNotWorkinWithinTags = new Dictionary<string, IList<string>>
    {
        { new BoldToken(0).Separator, new List<string> { new ItalicToken(0).Separator } }
    };

    public Type EscapeToken => typeof(EscapeToken);

    public ITag ConvertTag(Type type)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyDictionary<string, IList<string>> UnsupportedTags => tagNotWorkinWithinTags;
    public IReadOnlyDictionary<string, Func<int, IToken>> StringToToken => markdownToToken;
}