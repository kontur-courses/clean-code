namespace Markdown;

public sealed class TextRunRule : ITokenRule
{
    public Token TryReadTokenAndMoveCursor(TextCursor cursor)
    {
        if (cursor.End ||
            cursor.Current == '_'  ||
            cursor.Current == '\n' ||
            cursor.Current == '\r' ||
            cursor.Current == '\\' ||
            cursor.Current == ' '  ||
            char.IsDigit(cursor.Current))
            return null;

        var start = cursor.Position;
        while (!cursor.End)
        {
            var c = cursor.Current;
            if (c == '_' ||
                c == '\n' ||
                c == '\r' ||
                c == '\\' ||
                c == ' ' ||
                char.IsDigit(c))
                break;
            cursor.Move(1);
        }

        if (cursor.Position > start)
            return TokenFactory.Create(TokenType.Text, cursor.Slice(start, cursor.Position));

        return null;
    }
}