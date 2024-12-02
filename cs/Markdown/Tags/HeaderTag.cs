using Markdown.Tags.Enum;
using Markdown.Tokens;

namespace Markdown.Tags;

public class HeaderTag : ITag
{
    public TagType TagType => TagType.Header;
    private const char HeaderSymbol = '#';
    private const char EndHeaderTagSymbol = ' ';

    public bool IsCorrectOpenTag(IReadOnlyList<Token> tokens, int tokenPosition)
    {
        var token = tokens[tokenPosition];
        return IsOpenTag(token) && 
               (tokenPosition == 0 || tokens[tokenPosition - 1].Type == TokenType.NewLine);
    }

    public bool IsCorrectCloseTag(IReadOnlyList<Token> tokens, int tokenPosition)
    {
        var token = tokens[tokenPosition];
        return IsCloseTag(token);
    }

    public bool IsOpenTag(Token token)
    {
        var content = token.Content;
        return token.Type == TokenType.Tag && IsTag(content);
    }

    public bool IsCloseTag(Token token) => 
        token.Type == TokenType.NewLine;

    public bool IsTag(string content) => 
        content is [HeaderSymbol, EndHeaderTagSymbol];

    public bool IsStartOfTag(string content)
    {
        if (content.Length is 0 or > 2)
            return false;

        return content[0] == HeaderSymbol && (content.Length == 1 || content[1] == EndHeaderTagSymbol);
    }

    public TagContentProblem IsHaveProblemWithTags(IReadOnlyList<Token> tokens, int start, int end) => 
        TagContentProblem.None;
}