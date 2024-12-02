using Markdown.Tags.Enum;
using Markdown.Tokens;

namespace Markdown.Tags;

public class LinkValueTag : ITag
{
    public TagType TagType => TagType.LinkValue;
    private const string OpenTag = "(";
    private const string CloseTag = ")";
    private const int MinSymbolsCountBeforeTag = 2;
    public bool IsCorrectOpenTag(IReadOnlyList<Token> tokens, int tokenPosition)
    {
        var token = tokens[tokenPosition];
        var prevPos = tokenPosition - 1;
        return IsOpenTag(token) && 
               tokenPosition >= MinSymbolsCountBeforeTag && 
               tokens[prevPos].Type == TokenType.Tag && 
               IsCloseTagOfLinkText(tokens[prevPos]);
    }

    public bool IsCorrectCloseTag(IReadOnlyList<Token> tokens, int tokenPosition)
    {
        var token = tokens[tokenPosition];
        return IsCloseTag(token);
    }

    public bool IsOpenTag(Token token) => 
        token is { Type: TokenType.Tag, Content: OpenTag };

    public bool IsCloseTag(Token token) => 
        token is { Type: TokenType.Tag, Content: CloseTag };

    public bool IsTag(string content) => content is OpenTag or CloseTag;

    public bool IsStartOfTag(string content) => IsTag(content);

    public TagContentProblem IsHaveProblemWithTags(IReadOnlyList<Token> tokens, int openTagPosition, int closeTagPosition)
    {
        if (!IsOpenTag(tokens[openTagPosition]) || !IsCloseTag(tokens[closeTagPosition]))
            return TagContentProblem.NotTags;
        
        var prevPos = openTagPosition - 1;
        if (openTagPosition <= MinSymbolsCountBeforeTag || !IsCloseTagOfLinkText(tokens[prevPos]))
            return TagContentProblem.OpenTag;
        
        return TagContentProblem.None;
    }

    private static bool IsCloseTagOfLinkText(Token token) =>
        TokenUtilities.TryGetTagTypeByCloseTag(token, out var tagType) && tagType == TagType.LinkText;
}