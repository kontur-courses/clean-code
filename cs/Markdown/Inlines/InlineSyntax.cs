namespace Markdown.Inlines;

public static class InlineSyntax
{
    public const char Escape = '\\';
    public const char Underscore = '_';
    public const char Space = ' ';
    public const char Sharp = '#';
    public const char EndOfText = '\0';

    public const char PlaceholderUnderscore = '\uE000';
    public const char PlaceholderBackslash = '\uE001';
    public const char PlaceholderHash = '\uE002';

    public const string StrongMarker = "__";
    public const string EmMarker = "_";

    public static bool CanOpenOrCloseMarker(string text, int position, int length, bool open)
    {
        var prevChar = GetPrevChar(text, position);
        var nextChar = GetNextChar(text, position, length);

        if (open)
        {
            if (char.IsWhiteSpace(nextChar)) return false;
        }
        else
        {
            if (char.IsWhiteSpace(prevChar)) return false;
        }

        if (char.IsDigit(prevChar) && char.IsDigit(nextChar)) return false;

        return true;
    }

    public static char GetPrevChar(string text, int position)
    {
        return position - 1 >= 0 ? text[position - 1] : Space;
    }

    public static char GetNextChar(string text, int position, int length)
    {
        return position + length < text.Length ? text[position + length] : Space;
    }

    public static bool IsCrossingWords(bool openedInsideWord, bool closingInsideWord, bool sawWhitespace)
    {
        return openedInsideWord && closingInsideWord && sawWhitespace;
    }
}
