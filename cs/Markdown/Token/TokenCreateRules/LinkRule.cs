namespace Markdown;

public class LinkRule : ITokenRule
{
    public Token? TryReadTokenAndMoveCursor(TextCursor cursor)
    {
        var linkStart = cursor.Position;
        if (cursor.Current != '[')
            return null;

        var linkTextEnd = FindChar(cursor, ']');
        if (linkTextEnd == -1)
            return null;
        var linkText = cursor.Slice(linkStart + 1, linkTextEnd);
        cursor.Move(1); 

        var parenStart = cursor.Position;
        if (cursor.Current != '(')
        {
            return TokenFactory.Create(TokenType.Text, cursor.Slice(linkStart, linkTextEnd + 1));
        }

        var urlEnd = FindChar(cursor, ')');
        if (urlEnd == -1)
        {
            cursor.Revert(parenStart);
            return TokenFactory.Create(TokenType.Text, cursor.Slice(linkStart, linkTextEnd + 1));
        }

        cursor.Revert(parenStart + 1);
        if (!URL.ItsUrl(cursor))
        {
            cursor.Revert(linkStart);
            return null;
        }

        var linkArgs = cursor.Slice(parenStart + 1, urlEnd); 
        var url = linkArgs;
        var title = "";

        var titleStart = linkArgs.IndexOf('"');
        if (titleStart != -1)
        {
            url = linkArgs[..titleStart].Trim();
            int titleEnd = linkArgs.LastIndexOf('"');
            if (titleEnd > titleStart)
            {
                title = linkArgs.Substring(titleStart + 1, titleEnd - titleStart - 1);
            }
        }

        if (title.Length > 0)
            cursor.Move(title.Length + 3); 
        cursor.Move(1); 

        var tokenValue = $"{linkText}|{url}|{title}";
        return TokenFactory.Create(TokenType.Link, tokenValue);
    }

    private static int FindChar(TextCursor cursor, char targetChar)
    {
        while (!cursor.End)
        {
            cursor.Move(1);
            if (cursor.Current == targetChar)
            {
                return cursor.Position;
            }
        }
        return -1;
    }
}
