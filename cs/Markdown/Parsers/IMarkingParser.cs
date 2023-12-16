using Markdown.Tokens;

namespace Markdown.Parsers
{
    public interface IMarkingParser
    {
        IEnumerable<IToken> ParseText(string text);
    }
}
