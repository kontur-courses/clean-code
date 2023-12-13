using Markdown.Tokens;

namespace Markdown.Parsers
{
    public interface IParser
    {
        IEnumerable<IToken> ParseText(string text);
    }
}
