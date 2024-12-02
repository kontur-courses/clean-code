using Markdown.Tokens;

namespace Markdown.TokenParsers.TokenHandlers;

public class StrongTagsHandler : ITokenHandler
{
    public int Priority => 4;

    public IReadOnlyList<Token> Handle(IReadOnlyList<Token> tokens)
    {
        var handledTokens = new List<Token>();

        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            var nextPos = i + 1;
            var nextToken = nextPos < tokens.Count ? tokens[nextPos] : default;
            
            if ((IsStrongOpenTag(token, nextToken) || IsStrongCloseTag(token, nextToken)) && 
                TokenUtilities.TryCreateTagToken(token.Content + nextToken.Content, out var newStrongTagToken))
            {
                handledTokens.Add(newStrongTagToken!.Value);
                i = nextPos;
            }
            else
            {
                handledTokens.Add(token);
            }
        }
        
        return handledTokens;
    }
    
    private static bool IsStrongCloseTag(Token token, Token nextToken) => 
        TokenUtilities.TryGetTagTypeByCloseTag(token, out var currentTagType) && 
        TokenUtilities.TryGetTagTypeByCloseTag(nextToken, out var nextTagType) &&
        currentTagType == nextTagType &&
        currentTagType == TagType.Italic;

    private static bool IsStrongOpenTag(Token token, Token nextToken) =>
        TokenUtilities.TryGetTagTypeByOpenTag(token, out var currentTagType) && 
        TokenUtilities.TryGetTagTypeByOpenTag(nextToken, out var nextTagType) && 
        currentTagType == nextTagType && 
        currentTagType == TagType.Italic;
}