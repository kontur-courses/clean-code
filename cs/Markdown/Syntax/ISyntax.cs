using Markdown.Token;
using Markdown.Tag;

namespace Markdown.Syntax;

public interface ISyntax
{
    ITag ConvertTag(Type type);
    IReadOnlyDictionary<string, Func<int, IToken>> StringToToken { get; }
    Type EscapeToken { get; }
    IReadOnlyDictionary<string, IList<string>> UnsupportedTags { get; }
}