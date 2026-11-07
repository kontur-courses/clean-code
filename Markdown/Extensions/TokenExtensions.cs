using Markdown.Tokenization;

namespace Markdown.Extensions;

public static class TokenExtensions
{
    public static List<Token> Resolve(this IEnumerable<Token> unresolvedTokens)
        => Tokenizer.Resolve(unresolvedTokens);
}
