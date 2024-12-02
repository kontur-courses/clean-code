using Markdown.Tokens;

namespace Markdown.TokenParsers.TokenHandlers;

public class EscapeTokenApplyHandler : ITokenHandler
{
    public int Priority => 1;

    public IReadOnlyList<Token> Handle(IReadOnlyList<Token> tokens)
    {
        var handledTokens = new List<Token>();
        Token? previousToken = null;
        
        foreach (var token in tokens)
        {
            if (previousToken?.Type == TokenType.Escape)
            {
                if (token.Type is not (TokenType.Tag or TokenType.Escape) && 
                    TokenUtilities.TryCreateSeparatorToken(previousToken.Value.Content, out var punctuationToken))
                    handledTokens.Add(punctuationToken!.Value);

                handledTokens.Add(TokenUtilities.TryCreateSeparatorToken(token.Content, out var newCurrentToken)
                    ? newCurrentToken!.Value
                    : TokenUtilities.CreateTextToken(token.Content));
                previousToken = null;
            }
            else if (token.Type == TokenType.Escape)
            {
                previousToken = token;
            }
            else
            {
                handledTokens.Add(token);
                previousToken = token;
            }
        }
        
        if (previousToken?.Type == TokenType.Escape)
            handledTokens.Add(previousToken.Value);
        
        return handledTokens;
    }
}