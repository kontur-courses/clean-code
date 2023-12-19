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

    public Type EscapeToken => typeof(EscapeToken);

    public ITag ConvertTag(Type type)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyDictionary<string, Func<int, IToken>> StringToToken => markdownToToken;
}