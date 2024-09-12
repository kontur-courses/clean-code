using Markdown.Tokens;

namespace Markdown.Parsers;

public interface IMarkingParser
{
    IList<IToken> ParseText(string text);
}
