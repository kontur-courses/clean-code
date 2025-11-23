namespace Markdown;

public sealed class HeaderRule : ITokenRule
{
    public Token? TryReadTokenAndMoveCursor(TextCursor cursor)
    {
        if (!cursor.IsNewLine() || cursor.End) return null;
        
        if (cursor.Current == '#' && cursor.Peek() == ' ')
        {
            var currentPos = cursor.Position;
            cursor.Move(1);
            return TokenFactory.Create(TokenType.HeaderMarker, "#");
        }
        
        return null;
    }
}