using Markdown.Tags.Enum;
using Markdown.Tokens;

namespace Markdown.Tags;

public class LinkTextTag : ITag
{
    public TagType TagType => TagType.LinkText;
    private const string OpenTag = "[";
    private const string CloseTag = "]";
    private const int MinSymbolsCountAfterTag = 2;
    public bool IsCorrectOpenTag(IReadOnlyList<Token> tokens, int tokenPosition)
    {
        var token = tokens[tokenPosition];
        return IsOpenTag(token) && tokens.Count - 1 != tokenPosition;
    }

    public bool IsCorrectCloseTag(IReadOnlyList<Token> tokens, int tokenPosition)
    {
        var token = tokens[tokenPosition];
        var nextPos = tokenPosition + 1;
        return IsCloseTag(token) && tokens[nextPos].Type == TokenType.Tag && 
               tokenPosition <= tokens.Count - MinSymbolsCountAfterTag - 1 &&
               tokenPosition != 0 &&
               IsOpenTagOfLinkValue(tokens[nextPos]);
    }
    
    public bool IsOpenTag(Token token) =>
        token is { Type: TokenType.Tag, Content: OpenTag };

    public bool IsCloseTag(Token token) =>
        token is { Type: TokenType.Tag, Content: CloseTag };

    public bool IsTag(string content) => 
        content is OpenTag or CloseTag;

    public bool IsStartOfTag(string content) => IsTag(content);

    public TagContentProblem IsHaveProblemWithTags(IReadOnlyList<Token> tokens, int openTagPosition, int closeTagPosition)
    {
        if (!IsOpenTag(tokens[openTagPosition]) || !IsCloseTag(tokens[closeTagPosition]))
            return TagContentProblem.NotTags;
        
        var nextPos = closeTagPosition + 1;
        if (closeTagPosition > tokens.Count - 1 - MinSymbolsCountAfterTag || !IsOpenTagOfLinkValue(tokens[nextPos]))
            return TagContentProblem.CloseTag;
        
        return TagContentProblem.None;
    }

    private static bool IsOpenTagOfLinkValue(Token token) =>
        TokenUtilities.TryGetTagTypeByOpenTag(token, out var tagType) && tagType == TagType.LinkValue;
}