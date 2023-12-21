using Markdown.Tokens;

namespace Markdown.Filter;

public interface ITokenFilter
{
    public List<Token> FilterTokens(List<Token> tokens, string line);
}