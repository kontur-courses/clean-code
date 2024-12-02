using Markdown.Tokens;

namespace Markdown.TokenParsers.TokenHandlers;

public class IncorrectTagTokensHandler : ITokenHandler
{
    public int Priority => 2;
    public IReadOnlyList<Token> Handle(IReadOnlyList<Token> tokens)
    {
        var result = new List<Token>();
        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            
            if (token.Type != TokenType.Tag)
            {
                result.Add(token);
                continue;
            }
            
            if (TokenUtilities.TryGetTagByOpenTag(token, out var openTag) && openTag!.IsCorrectOpenTag(tokens, i) || 
                TokenUtilities.TryGetTagByCloseTag(token, out var closeTag) && closeTag!.IsCorrectCloseTag(tokens, i))
                result.Add(token);
            else
                result.Add(TokenUtilities.CreateTextToken(token.Content));
        }
        
        return result;
    }
}