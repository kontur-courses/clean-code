using Markdown.Extensions;
using Markdown.Tokens;

namespace Markdown.TokenParsers.TokenReaders;

public class CommonTokensReader : ITokenReader
{
    public IEnumerable<Token> ReadTokens(string text)
    {
        for (var i = 0; i < text.Length; i++)
        {
            var currentChar = text.GetSymbol(i);

            if (TokenUtilities.TryCreateCommonToken(currentChar, out var commonToken))
                yield return commonToken!.Value;
            else if (TokenUtilities.TryCreateSeparatorToken(currentChar, out var punctuationToken))
                yield return punctuationToken!.Value;
            else
            {
                var tokenType = TokenUtilities.IsTagStartPart(currentChar) ? TokenType.Tag : TokenType.Word;
                yield return CreateTokenAndMovePointer(text, tokenType, ref i);
            }
        }
        
        yield return TokenUtilities.EndOfText;
    }
    
    private static Token CreateTokenAndMovePointer(string text, TokenType tokenType, ref int i)
    {
        var content = "";

        while (i < text.Length && !IsTokenEnded(text.GetSymbol(i), content, tokenType))
        {
            content += text[i];
            i++;
        }

        i--;

        if (tokenType == TokenType.Tag && TokenUtilities.TryCreateTagToken(content, out var token))
            return token!.Value;

        return TokenUtilities.CreateTextToken(content);
    }

    private static bool IsTokenEnded(string nextChar, string content, TokenType tokenType)
    {
        if (tokenType == TokenType.Word &&
            (TokenUtilities.TryCreateCommonToken(nextChar, out _) || TokenUtilities.IsTagStartPart(nextChar)))
            return true;
        return TokenUtilities.TryCreateTagToken(content, out _);
    }
}