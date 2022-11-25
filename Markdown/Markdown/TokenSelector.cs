using Markdown.Tokens;

namespace Markdown;

public static class TokenSelector
{
    public static IReadOnlyList<Token> Tokens => new List<Token>
    {
        new HeaderToken(),
        new BoldToken(),
        new ItalicToken(),
        new EscapeToken(),
        new UnorderedListItem()
    };

    public static Token? SelectLongestSuitableToken(string line, int index)
    {
        var tokensToCheck = Tokens;
        var suitableToken = tokensToCheck
            .Where(token => token.Opening.IsSubstringAt(line, index))
            .MaxBy(token => token.Opening.Length);
        return suitableToken;
    }
}