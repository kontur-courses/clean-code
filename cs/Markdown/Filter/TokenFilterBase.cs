using Markdown.Tokens;
using Markdown.Tokens.Decorators;

namespace Markdown.Filter;

public abstract class TokenFilterBase : ITokenFilter
{
    public abstract List<Token> FilterTokens(List<Token> tokens, string line);

    protected static List<TokenFilteringDecorator> PackTokensForFiltering(IEnumerable<Token> tokens)
        => tokens
            .Select(token => new TokenFilteringDecorator(token))
            .ToList();

    protected static List<Token> UnpackFilteredTokens(IEnumerable<TokenFilteringDecorator> tokens)
        => tokens
            .Select(token => new Token(token))
            .ToList();
}