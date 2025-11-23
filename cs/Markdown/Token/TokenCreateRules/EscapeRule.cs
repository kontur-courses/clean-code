namespace Markdown;

public sealed class EscapeRule : ITokenRule
{
    public Token? TryReadTokenAndMoveCursor(TextCursor cursor)
    {
        if (cursor.End) return null;
        if (cursor.Current != '\\') return null;

        var pos = cursor.Position;
        cursor.Move(1);
        if (!cursor.End)
        {
            var s = cursor.Current.ToString();
            cursor.Move(1);
            return TokenFactory.Create(TokenType.Text, s);
        }

        return TokenFactory.Create(TokenType.Text, "\\");
    }
}