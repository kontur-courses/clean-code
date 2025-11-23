namespace Markdown;

public sealed class WhiteSpaceRule : ITokenRule
{
    public Token? TryReadTokenAndMoveCursor(TextCursor cursor)
    {
        if (cursor.Current != ' ' || cursor.End)
            return null;
        
        var start = cursor.Position;

        while (!cursor.End)
        {
            if (cursor.Current != ' ')
                break;
            cursor.Move(1);
        }
        
        var value = cursor.Slice(start, cursor.Position);
        return TokenFactory.Create(TokenType.WhiteSpace,value);
    }
}