using Markdown.Tokens.StringToken;

namespace Markdown.Parser;

public interface IParser
{
    public IEnumerable<StringToken> Parse(string input);
}