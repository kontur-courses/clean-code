using Markdown.Tokenization;
using Markdown.Tokenization.Tokens.Abstractions;

namespace Markdown.Extensions;

public static class TokenExtensions
{
    public static List<Token> Resolve(this IEnumerable<Token> unresolvedTokens)
        => Tokenizer.Resolve(unresolvedTokens);
}
