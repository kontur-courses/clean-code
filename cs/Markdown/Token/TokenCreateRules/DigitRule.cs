namespace Markdown;

public class DigitRule : ITokenRule
{
    public Token? TryReadTokenAndMoveCursor(TextCursor cursor)
    {
        if (!char.IsDigit(cursor.Current) || cursor.End)
            return null;
        
        var start = cursor.Position;

        while (!cursor.End)
        {
            if (!char.IsDigit(cursor.Current))
                break;
            cursor.Move(1);
        }

        var value = cursor.Slice(start, cursor.Position);
        return TokenFactory.Create(TokenType.WhiteSpace, value);
    }
}