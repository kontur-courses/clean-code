namespace Markdown;

public sealed class ScoreContext
{
    public List<Node> Children { get; }
    public TokenCursor Cursor { get; }
    public Token CurrentToken { get; }
    public TokenType CurrentTokenType { get; }
    public int CurrentTokenPos { get; }
    public NodeType CurrentNodeType { get; }
    
    public List<(int Index, TokenType Type)> InnerScore { get; } = new();
    public int CloseIndex { get; set; } = -1;

    public ScoreContext(List<Node> children, TokenCursor cursor)
    {
        Children = children;
        Cursor = cursor;
        CurrentToken = cursor.Current;
        CurrentTokenType = CurrentToken.Type;
        CurrentTokenPos = cursor.Position;
        CurrentNodeType = CurrentTokenType == TokenType.DoubleUnderscore
            ? NodeType.Strong
            : NodeType.Emphasis;
    }
}