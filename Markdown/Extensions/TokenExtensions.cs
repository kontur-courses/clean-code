using Markdown.Tokenization;
using Markdown.Tokenization.Tokens;

namespace Markdown.Extensions;

public static class TokenExtensions
{
    public static ResolvedTokens Resolve(this UnresolvedTokens unresolvedTokens)
        => new TokenResolver().Resolve(unresolvedTokens);
}
