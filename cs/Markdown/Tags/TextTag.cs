using Markdown.Tags.Enum;
using Markdown.Tokens;

namespace Markdown.Tags;

public abstract class TextTag : ITag
{
    public abstract TagType TagType { get; }
    protected abstract string TagContent { get; }

    public bool IsCorrectOpenTag(IReadOnlyList<Token> tokens, int tokenPosition)
    {
        var token = tokens[tokenPosition];
        if (!IsOpenTag(token))
            return false;
        
        var nextPos = tokenPosition + 1;
        return IsCorrectTag(tokens, tokenPosition) && 
               (nextPos != tokens.Count && tokens[nextPos].Type != TokenType.Space);
    }

    public bool IsCorrectCloseTag(IReadOnlyList<Token> tokens, int tokenPosition)
    {
        var token = tokens[tokenPosition];
        if (!IsCloseTag(token))
            return false;
        
        var previousPos = tokenPosition - 1;
        return IsCorrectTag(tokens, tokenPosition) && 
               previousPos > 0 && tokens[previousPos].Type != TokenType.Space;
    }

    private static bool IsCorrectTag(IReadOnlyList<Token> tokens, int tokenPosition)
    {
        var previousPos = tokenPosition - 1;
        var nextPos = tokenPosition + 1;

        return (previousPos < 0 || tokens[previousPos].Type != TokenType.Digit) && 
               (nextPos >= tokens.Count || tokens[nextPos].Type != TokenType.Digit);
    }
    
    public bool IsOpenTag(Token token) => IsTag(token);

    public bool IsCloseTag(Token token) => IsTag(token);

    private bool IsTag(Token token) => 
        token.Type == TokenType.Tag && token.Content == TagContent;

    public bool IsTag(string content) => content == TagContent;

    public abstract bool IsStartOfTag(string content);

    public TagContentProblem IsHaveProblemWithTags(IReadOnlyList<Token> tokens, int start, int end)
    {
        if (!IsOpenTag(tokens[start]) || !IsCloseTag(tokens[end]))
            return TagContentProblem.NotTags;
        
        if(!TagsHaveContent(tokens, start, end))
            return TagContentProblem.NoContent;

        return IsWordPartInTags(tokens, start, end) ? 
            TagContentProblem.None : 
            TagMarkValidPhrase(tokens, start, end);
    }

    private static bool TagsHaveContent(IReadOnlyList<Token> tokens, int start, int end)
    {
        var currentPos = start;
        var isHaveContent = false;
        while (currentPos < end)
            if (tokens[++currentPos].Type != TokenType.Tag)
            {
                isHaveContent = true;
                break;
            }
                
        return isHaveContent;
    }

    private static bool IsWordPartInTags(IReadOnlyList<Token> tokens, int start, int end) =>
        end - start == 2 && tokens[start + 1].Type == TokenType.Word;
    
    private static TagContentProblem TagMarkValidPhrase(IReadOnlyList<Token> tokens, int prevPos, int nextPos)
    {
        while (prevPos >= 0 && tokens[prevPos].Type == TokenType.Tag)
            prevPos--;
        if (prevPos >= 0 && tokens[prevPos].Type == TokenType.Word)
            return TagContentProblem.OpenTag;
        
        while (nextPos < tokens.Count && tokens[nextPos].Type == TokenType.Tag)
            nextPos++;
        if (nextPos < tokens.Count && tokens[nextPos].Type == TokenType.Word)
            return TagContentProblem.CloseTag;

        return TagContentProblem.None;
    }
}