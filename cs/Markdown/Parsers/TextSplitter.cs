using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Parsers;

public class TextSplitter
{
    private int currentPosition;

    public TextSplitter(int position)
    {
        currentPosition = position;
    }

    public IEnumerable<IToken> SplitOnTokens(string text)
    {
        for (; currentPosition < text.Length; currentPosition++)
        {
            var tokenType = GetTokenType(text[currentPosition]);
            yield return CreateTokenAndMoveToNext(text, tokenType, currentPosition);
        }
    }

    private TokenType GetTokenType(char startSymbol)
    {
        if (SupportedTags.Tags.TryGetValue(startSymbol.ToString(), out var tag) && tag.TagType == TagType.Escape)
            return TokenType.Escape;
        var isTag = TokenUtilities.IsTagStartingWithSymbol(startSymbol);
        return isTag ? TokenType.Tag : TokenType.Text;
    }

    private IToken CreateTokenAndMoveToNext(string text, TokenType type, int startPosition)
    {
        var tokenContent = "";

        while (currentPosition < text.Length && NeedCreateToken(type, text[currentPosition], tokenContent))
        {
            tokenContent += text[currentPosition];
            currentPosition++;
        }

        currentPosition--;

        return new Token(tokenContent, type, startPosition);
    }

    private bool NeedCreateToken(TokenType tokenType, char currentSymbol, string previousSymbols)
    {
        return tokenType switch
        {
            TokenType.Text => !TokenUtilities.IsTextTokenEnd(currentSymbol),
            TokenType.Tag => !TokenUtilities.IsTagTokenEnd(previousSymbols),
            TokenType.Escape => !TokenUtilities.IsEscapeTokenEnd(previousSymbols),
            _ => throw new ArgumentException($"token with type {tokenType} not supported")
        };
    }
}
