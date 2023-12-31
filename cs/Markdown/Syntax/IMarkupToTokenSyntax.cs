using Markdown.Token;

namespace Markdown.Syntax;

public interface IMarkupToTokenSyntax
{
    IReadOnlyDictionary<string, Func<int, IToken>> StringToToken { get; }
    Type EscapeToken { get; }
    string[] NewLineSeparators { get; }
    IReadOnlyDictionary<string, IList<string>> TagCannotBeInsideTags { get; }
}