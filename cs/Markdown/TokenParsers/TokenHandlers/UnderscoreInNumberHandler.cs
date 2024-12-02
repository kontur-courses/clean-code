using System.Text;
using Markdown.Extensions;
using Markdown.Tokens;

namespace Markdown.TokenParsers.TokenHandlers;

public class UnderscoreInNumberHandler : ITokenHandler
{
    public int Priority => 3;

    public IReadOnlyList<Token> Handle(IReadOnlyList<Token> tokens)
    {
        var tokenQueue = new Queue<Token>(tokens);
        var handledTokens = new List<Token> { tokenQueue.Dequeue() };
        Token? previousToken = tokens[0];

        while (tokenQueue.Count > 0)
        {
            var currentToken = tokenQueue.Peek();
            if (IsNotStrongOrItalicTag(currentToken) || 
                previousToken is not { Type: TokenType.Digit })
            {
                handledTokens.Add(tokenQueue.Dequeue());
                previousToken = currentToken;
            }
            else
            {
                var (token, length) = MoveToNotStrongOrItalicTagToken(tokenQueue);

                if (token?.Type == TokenType.Digit)
                {
                    var newToken = CombineTokensInOne(tokenQueue, length);
                    handledTokens.Add(newToken);
                }
                else
                    handledTokens.AddRange(tokenQueue.Dequeue(length));
                previousToken = null;
            }
        }
        
        return handledTokens;
    }

    private static (Token?, int position) MoveToNotStrongOrItalicTagToken(IEnumerable<Token> tokens)
    {
        var i = 0;
        
        foreach (var token in tokens)
        {
            if (IsNotStrongOrItalicTag(token))
                return (token, i);
                
            i++;
        }

        return (null, -1);
    }

    private static bool IsNotStrongOrItalicTag(Token token) =>
        token.Type != TokenType.Tag || 
        !TokenUtilities.TryGetTagTypeByOpenTag(token, out var tagType) || 
        tagType is not TagType.Italic and not TagType.Strong;
    
    private static Token CombineTokensInOne(Queue<Token> tokens, int length)
    {
        var stringBuilder = new StringBuilder();

        var tokensContent = tokens.Dequeue(length).Select(token => token.Content);
        stringBuilder.Append(tokensContent);
        
        return TokenUtilities.CreateTextToken(stringBuilder.ToString());
    }
}