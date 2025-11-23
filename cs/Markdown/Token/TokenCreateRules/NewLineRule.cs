namespace Markdown;

public sealed class NewLineRule : ITokenRule
{
    public Token TryReadTokenAndMoveCursor(TextCursor cursor)
    {
        if (cursor.End) return null;
        if (cursor.Current == '\n')
        {
            var pos = cursor.Position;
            cursor.Move(1);
            return TokenFactory.Create(TokenType.NewLine, "\n");
        }

        if (cursor.Current == '\r')
        {
            cursor.Move(1);
        }

        return null;
    }
}